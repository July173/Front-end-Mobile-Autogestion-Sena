using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutogestionSenaMaui.ViewModels;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;
using Microsoft.Maui.Controls;
using AutogestionSenaMaui.Helpers;
using AutogestionSena.MAUI.Api;

namespace AutogestionSenaMaui.Views
{
    public partial class AdminDashboardPage : ContentPage
    {
        private readonly AdminDashboardViewModel _viewModel;
        private readonly MenuService _menuService;
        private readonly AssignmentService _assignmentService;
        private readonly ApprenticeSimpleService _apprenticeService;
        private readonly DashboardCardsViewModel _cardsViewModel;

        // Timer para refresco automático
        private System.Timers.Timer? _autoRefreshTimer;

        public AdminDashboardPage()
        {
            InitializeComponent();
            _viewModel = new AdminDashboardViewModel();
            _menuService = new MenuService();
            _assignmentService = new AssignmentService();
            _apprenticeService = new ApprenticeSimpleService();
            _cardsViewModel = new DashboardCardsViewModel();
            BindingContext = _cardsViewModel; // Cambiar BindingContext para las cards
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await _viewModel.LoadDataAsync();
                await LoadDebugDataAsync();
                await LoadStatisticsAsync();
                await _cardsViewModel.LoadAsync();

                // Iniciar refresco automático cada 30 segundos
                if (_autoRefreshTimer == null)
                {
                    _autoRefreshTimer = new System.Timers.Timer(30000); // 30 segundos
                    _autoRefreshTimer.Elapsed += async (s, e) =>
                    {
                        // Ejecutar en el hilo principal de UI
                        await MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            LoadingStatusLabel.Text = "⏳ Refrescando datos automáticamente...";
                            LoadingStatusLabel.TextColor = Colors.Orange;
                            await LoadStatisticsAsync();
                        });
                    };
                    _autoRefreshTimer.AutoReset = true;
                    _autoRefreshTimer.Start();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] Error cargando datos: {ex}");
            }
            // Configurar breadcrumb en el MainLayout TopBar
            MainLayoutHelper.UpdateCurrentBreadcrumb("Dashboard", "Administración");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_autoRefreshTimer != null)
            {
                _autoRefreshTimer.Stop();
                _autoRefreshTimer.Dispose();
                _autoRefreshTimer = null;
            }
            // (TopBar is in MainLayout; menu toggling is handled by MainLayoutPage)
        }

        // Removed local OnTopBarMenuClicked: handled by MainLayoutPage globally

        /// <summary>
        /// Carga los datos de debug: usuario y menú
        /// </summary>
        private async Task LoadDebugDataAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] 🔍 Cargando datos de debug...");

                // 1. Leer datos del usuario desde Preferences
                var userId = Preferences.Get("UserId", 0);
                var userRole = Preferences.Get("UserRole", 0);
                var userEmail = Preferences.Get("UserEmail", string.Empty);
                var authToken = Preferences.Get("AuthToken", string.Empty);

                System.Diagnostics.Debug.WriteLine("===========================================");
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] 📊 DATOS DE PREFERENCES:");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] UserId = {userId}");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] UserRole = {userRole}");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] UserEmail = {userEmail}");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] AuthToken = {(string.IsNullOrEmpty(authToken) ? "VACÍO" : "OK")}");
                System.Diagnostics.Debug.WriteLine("===========================================");

                
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] UserId={userId}, Role={userRole}, Email={userEmail}");

                // 2. Cargar menú desde la API
                // Si UserId es 0, intentar usar UserPerson como respaldo
                int idParaMenu = userId;
                
                if (idParaMenu == 0)
                {
                    var userPerson = Preferences.Get("UserPerson", 0);
                    if (userPerson > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ⚠️ UserId es 0, usando UserPerson={userPerson} como respaldo");
                        idParaMenu = userPerson;
                    }
                    else if (userRole > 0)
                    {
                        // Como último recurso, usar el UserRole como ID
                        // NOTA: Esto es temporal hasta que el backend envíe el UserId correcto
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ⚠️ UserId y UserPerson son 0, usando UserRole={userRole} como ID temporal");
                        idParaMenu = userRole;
                    }
                }
                
                if (idParaMenu > 0)
                {
                   
                    try
                    {
                        var menuData = await _menuService.GetMenuItemsAsync(idParaMenu.ToString());

                        if (menuData != null && menuData.MenuItems != null && menuData.MenuItems.Any())
                        {
                            // Mostrar cada módulo y sus formularios
                            foreach (var module in menuData.MenuItems.OrderBy(m => m.Order))
                            {
                                // Módulo
                                var moduleFrame = new Frame
                                {
                                    BackgroundColor = Color.FromArgb("#F8F9FA"),
                                    CornerRadius = 4,
                                    Padding = 8,
                                    Margin = new Thickness(0, 4),
                                    HasShadow = false
                                };

                                var moduleStack = new VerticalStackLayout { Spacing = 4 };

                                moduleStack.Children.Add(new Label
                                {
                                    Text = $"📦 {module.Name}",
                                    FontAttributes = FontAttributes.Bold,
                                    TextColor = Color.FromArgb("#2D7430"),
                                    FontSize = 14
                                });

                                // Formularios del módulo
                                if (module.SubMenus != null && module.SubMenus.Any())
                                {
                                    foreach (var form in module.SubMenus.OrderBy(f => f.Order))
                                    {
                                        moduleStack.Children.Add(new Label
                                        {
                                            Text = $"   └─ {form.Name} → {form.BackendPath ?? form.Route}",
                                            TextColor = Color.FromArgb("#555555"),
                                            FontSize = 13,
                                            Margin = new Thickness(12, 2, 0, 0)
                                        });
                                    }
                                }
                                else
                                {
                                    moduleStack.Children.Add(new Label
                                    {
                                        Text = "   (sin formularios)",
                                        TextColor = Color.FromArgb("#999999"),
                                        FontSize = 12,
                                        FontAttributes = FontAttributes.Italic,
                                        Margin = new Thickness(12, 2, 0, 0)
                                    });
                                }

                                moduleFrame.Content = moduleStack;
                               }

                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ✅ Menú mostrado: {menuData.MenuItems.Count} módulos");
                        }
                        else
                        {
                          System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ Menú vacío");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ Error cargando menú: {ex}");

                    }
                }
                else
                {
                   System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ No se puede cargar menú - UserId={userId}, UserPerson={Preferences.Get("UserPerson", 0)}, UserRole={userRole}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ Error en LoadDebugDataAsync: {ex}");
            }
        }

       
        /// <summary>
        /// Obtiene el nombre del rol
        /// </summary>
        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Administrador",
                2 => "Aprendiz",
                3 => "Instructor",
                4 => "Coordinador",
                5 => "Operador SofiaPlus",
                _ => "Desconocido"
            };
        }

        /// <summary>
        /// Extrae el nombre del usuario desde el email
        /// </summary>
        private string GetUserNameFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return "N/A";

            try
            {
                var emailPart = email.Split('@')[0];
                var nameParts = emailPart.Split('.');

                if (nameParts.Length >= 2)
                {
                    var firstName = char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1).ToLower();
                    var lastName = char.ToUpper(nameParts[1][0]) + nameParts[1].Substring(1).ToLower();
                    return $"{firstName} {lastName}";
                }
                else if (nameParts.Length == 1)
                {
                    return char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1).ToLower();
                }
            }
            catch { }

            return email;
        }

        /// <summary>
        /// Carga las estadísticas de aprendices y asignaciones
        /// </summary>
        private async Task LoadStatisticsAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] 📊 Cargando estadísticas...");

                // Configurar token de autenticación
                var authToken = Preferences.Get("AuthToken", string.Empty);
                if (!string.IsNullOrEmpty(authToken))
                {
                    _assignmentService.SetAuthToken(authToken);
                    _apprenticeService.SetAuthToken(authToken);
                }

                System.Diagnostics.Debug.WriteLine("===========================================");
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] 📊 INICIANDO CARGA DE ESTADÍSTICAS");
                System.Diagnostics.Debug.WriteLine("===========================================");

                // Actualizar estado visual
                LoadingStatusLabel.Text = "⏳ Cargando datos del backend...";
                LoadingStatusLabel.TextColor = Colors.Orange;

                // CARGAR APRENDICES
                System.Diagnostics.Debug.WriteLine("\n[AdminDashboard] 👥 --- CARGANDO APRENDICES ---");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] 🌐 Endpoint: {Endpoints.ApprenticeSimple.GetAllApprenticesSimple}");
                
                ApprenticesEndpointLabel.Text = $"Endpoint: {Endpoints.ApprenticeSimple.GetAllApprenticesSimple}";
                
                List<ApprenticeSimpleDto>? apprentices = null;
                try
                {
                    apprentices = await _apprenticeService.GetAllApprenticesAsync();
                    
                    if (apprentices != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ✅ Respuesta recibida: {apprentices.Count} aprendices");
                        
                        var activeCount = apprentices.Count(a => a.Active);
                        var inactiveCount = apprentices.Count(a => !a.Active);
                        
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Activos: {activeCount}");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Inactivos: {inactiveCount}");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Total: {apprentices.Count}");
                        
                        // Actualizar labels de debug
                        ApprenticesTotalLabel.Text = $"✅ Total recibidos: {apprentices.Count}";
                        ApprenticesTotalLabel.TextColor = Colors.Green;
                        ApprenticesActiveLabel.Text = $"   Activos: {activeCount}";
                        ApprenticesInactiveLabel.Text = $"   Inactivos: {inactiveCount}";
                        
                        // Mostrar primeros 3 aprendices como muestra
                        var samples = new List<string>();
                        foreach (var app in apprentices.Take(3))
                        {
                            samples.Add($"ID={app.Id}, Person={app.Person}, Active={app.Active}");
                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Ejemplo: {samples.Last()}");
                        }
                        ApprenticeSampleLabel.Text = $"Ejemplos:\n{string.Join("\n", samples)}";
                        
                        TotalApprenticesLabel.Text = activeCount.ToString("N0");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] 🎯 Label actualizado con: {activeCount}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ Respuesta NULL del servicio de aprendices");
                        TotalApprenticesLabel.Text = "0";
                        ApprenticesTotalLabel.Text = "⚠️ Respuesta NULL del servicio";
                        ApprenticesTotalLabel.TextColor = Colors.Orange;
                        ApprenticesActiveLabel.Text = "-";
                        ApprenticesInactiveLabel.Text = "-";
                        ApprenticeSampleLabel.Text = "No hay datos disponibles";
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ ERROR al cargar aprendices:");
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Tipo: {ex.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Mensaje: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    StackTrace: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    InnerException: {ex.InnerException.Message}");
                    }
                    
                    TotalApprenticesLabel.Text = "Error";
                    ApprenticesTotalLabel.Text = $"❌ ERROR: {ex.Message}";
                    ApprenticesTotalLabel.TextColor = Colors.Red;
                    ApprenticesActiveLabel.Text = $"Tipo: {ex.GetType().Name}";
                    ApprenticesInactiveLabel.Text = "-";
                    ApprenticeSampleLabel.Text = $"Detalles:\n{ex.Message}";
                }

                // CARGAR ASIGNACIONES
                System.Diagnostics.Debug.WriteLine("\n[AdminDashboard] 📋 --- CARGANDO ASIGNACIONES ---");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] 🌐 Endpoint: {Endpoints.Assignment.GetFormRequestList}");
                
                AssignmentsEndpointLabel.Text = $"Endpoint: {Endpoints.Assignment.GetFormRequestList}";
                
                AssignmentRequestListResponse? assignments = null;
                int assignedCount = 0;
                int unassignedCount = 0;
                
                try
                {
                    assignments = await _assignmentService.GetFormRequestListAsync();
                    
                    if (assignments != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ✅ Respuesta recibida:");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Success: {assignments.Success}");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Message: {assignments.Message}");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Count: {assignments.Count}");
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Data es null: {assignments.Data == null}");
                        
                        if (assignments.Success && assignments.Data != null)
                        {
                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Data.Count: {assignments.Data.Count}");
                            
                            // Actualizar labels de debug
                            AssignmentsTotalLabel.Text = $"✅ Total recibidas: {assignments.Data.Count}";
                            AssignmentsTotalLabel.TextColor = Colors.Green;
                            AssignmentsSuccessLabel.Text = $"   Success: {assignments.Success} | Message: {assignments.Message}";
                            
                            // Contar por estado
                            var porEstado = assignments.Data
                                .GroupBy(a => a.RequestState ?? "NULL")
                                .Select(g => new { Estado = g.Key, Count = g.Count() })
                                .ToList();
                            
                            var estadosTexto = string.Join(", ", porEstado.Select(e => $"{e.Estado}: {e.Count}"));
                            AssignmentsByStateLabel.Text = $"Por estado:\n{estadosTexto}";
                            
                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Distribución por estado:");
                            foreach (var estado in porEstado)
                            {
                                System.Diagnostics.Debug.WriteLine($"[AdminDashboard]      - {estado.Estado}: {estado.Count}");
                            }
                            
                            // Contar asignadas (todas las que tienen estado ASIGNADO o APROBADO)
                            assignedCount = assignments.Data.Count(a => a.RequestState == "ASIGNADO" || a.RequestState == "APROBADO");
                            unassignedCount = assignments.Data.Count(a => a.RequestState == "SIN_ASIGNAR");
                            
                            TotalAssignmentsLabel.Text = assignedCount.ToString();
                            UnassignedCountLabel.Text = unassignedCount.ToString();
                            
                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard] 🎯 Labels actualizados:");
                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Asignadas: {assignedCount}");
                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    - Sin asignar: {unassignedCount}");
                            
                            // Mostrar primeros 3 como muestra
                            var samples = new List<string>();
                            foreach (var asig in assignments.Data.Take(3))
                            {
                                samples.Add($"ID={asig.Id}, Estado={asig.RequestState}, Fecha={asig.FechaSolicitud}");
                                System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Ejemplo: {samples.Last()}");
                            }
                            AssignmentSampleLabel.Text = $"Ejemplos:\n{string.Join("\n", samples)}";
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ Success=false o Data=null");
                            TotalAssignmentsLabel.Text = "0";
                            UnassignedCountLabel.Text = "0";
                            AssignmentsTotalLabel.Text = "⚠️ Success=false o Data=null";
                            AssignmentsTotalLabel.TextColor = Colors.Orange;
                            AssignmentsSuccessLabel.Text = $"Success: {assignments?.Success}";
                            AssignmentsByStateLabel.Text = "-";
                            AssignmentSampleLabel.Text = "No hay datos disponibles";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ Respuesta NULL del servicio de asignaciones");
                        TotalAssignmentsLabel.Text = "0";
                        UnassignedCountLabel.Text = "0";
                        AssignmentsTotalLabel.Text = "⚠️ Respuesta NULL del servicio";
                        AssignmentsTotalLabel.TextColor = Colors.Orange;
                        AssignmentsSuccessLabel.Text = "-";
                        AssignmentsByStateLabel.Text = "-";
                        AssignmentSampleLabel.Text = "No hay datos disponibles";
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ ERROR al cargar asignaciones:");
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Tipo: {ex.GetType().Name}");
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    Mensaje: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    StackTrace: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard]    InnerException: {ex.InnerException.Message}");
                    }
                    
                    TotalAssignmentsLabel.Text = "Error";
                    UnassignedCountLabel.Text = "Error";
                    AssignmentsTotalLabel.Text = $"❌ ERROR: {ex.Message}";
                    AssignmentsTotalLabel.TextColor = Colors.Red;
                    AssignmentsSuccessLabel.Text = $"Tipo: {ex.GetType().Name}";
                    AssignmentsByStateLabel.Text = "-";
                    AssignmentSampleLabel.Text = $"Detalles:\n{ex.Message}";
                }

                // Cargar gráficas
                System.Diagnostics.Debug.WriteLine("\n[AdminDashboard] 📊 --- CARGANDO GRÁFICAS ---");
                LoadCharts(assignedCount, unassignedCount);

                // Actualizar estado final
                LoadingStatusLabel.Text = "✅ Datos cargados exitosamente";
                LoadingStatusLabel.TextColor = Colors.Green;

                System.Diagnostics.Debug.WriteLine("\n===========================================");
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] ✅ ESTADÍSTICAS CARGADAS");
                System.Diagnostics.Debug.WriteLine("===========================================");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("\n===========================================");
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] ❌ ERROR GENERAL");
                System.Diagnostics.Debug.WriteLine("===========================================");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] Tipo: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] Mensaje: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] StackTrace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard] InnerException: {ex.InnerException.Message}");
                }
                
                TotalApprenticesLabel.Text = "Error";
                TotalAssignmentsLabel.Text = "Error";
                UnassignedCountLabel.Text = "Error";
            }
        }

        /// <summary>
        /// Carga las gráficas de barras y líneas con datos reales
        /// </summary>
        private async void LoadCharts(int assigned, int unassigned)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[AdminDashboard] 📊 Cargando gráficas con datos reales...");

                // Configurar token de autenticación
                var authToken = Preferences.Get("AuthToken", string.Empty);
                if (!string.IsNullOrEmpty(authToken))
                {
                    _assignmentService.SetAuthToken(authToken);
                }

                // Obtener todas las asignaciones
                var response = await _assignmentService.GetFormRequestListAsync();
                
                if (response != null && response.Success && response.Data != null)
                {
                    var assignments = response.Data;
                    
                    // GRÁFICA 1: Asignaciones por estado (Barras verdes)
                    var statusCounts = assignments
                        .GroupBy(a => a.RequestState ?? "DESCONOCIDO")
                        .Select(g => new { Status = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .ToList();

                    var assignmentEntries = new List<Microcharts.ChartEntry>();
                    foreach (var item in statusCounts)
                    {
                        assignmentEntries.Add(new Microcharts.ChartEntry(item.Count)
                        {
                            Label = GetShortStatus(item.Status),
                            ValueLabel = item.Count.ToString(),
                            Color = SkiaSharp.SKColor.Parse("#4CAF50")
                        });
                    }

                    // Si no hay datos, mostrar un mensaje
                    if (!assignmentEntries.Any())
                    {
                        assignmentEntries.Add(new Microcharts.ChartEntry(0)
                        {
                            Label = "Sin datos",
                            ValueLabel = "0",
                            Color = SkiaSharp.SKColor.Parse("#CCCCCC")
                        });
                    }

                    AssignmentsChart.Chart = new Microcharts.BarChart
                    {
                        Entries = assignmentEntries,
                        BackgroundColor = SkiaSharp.SKColors.White,
                        LabelTextSize = 28,
                        ValueLabelOrientation = Microcharts.Orientation.Horizontal,
                        LabelOrientation = Microcharts.Orientation.Horizontal,
                        IsAnimated = true
                    };

                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ✅ Gráfica de barras: {assignmentEntries.Count} estados");

                    // GRÁFICA 2: Tendencia de asignaciones por mes (Línea azul)
                    // Agrupar todas las asignaciones por mes (últimos 6 meses)
                    var allByMonth = assignments
                        .Where(a => a.CreatedAt.HasValue)
                        .GroupBy(a => new { Year = a.CreatedAt.Value.Year, Month = a.CreatedAt.Value.Month })
                        .Select(g => new
                        {
                            Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                            Count = g.Count()
                        })
                        .OrderBy(x => x.Date)
                        .TakeLast(6) // Últimos 6 meses
                        .ToList();

                    var approvedEntries = new List<Microcharts.ChartEntry>();
                    
                    if (allByMonth.Any())
                    {
                        foreach (var item in allByMonth)
                        {
                            approvedEntries.Add(new Microcharts.ChartEntry(item.Count)
                            {
                                Label = item.Date.ToString("MMM"),
                                ValueLabel = item.Count.ToString(),
                                Color = SkiaSharp.SKColor.Parse("#2196F3")
                            });
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] 📈 Datos por mes: {string.Join(", ", allByMonth.Select(x => $"{x.Date:MMM}: {x.Count}"))}");
                    }
                    else
                    {
                        // Datos por defecto si no hay ninguna fecha
                        approvedEntries.Add(new Microcharts.ChartEntry(0)
                        {
                            Label = "Sin datos",
                            ValueLabel = "0",
                            Color = SkiaSharp.SKColor.Parse("#CCCCCC")
                        });
                        
                        System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ No hay datos con fechas válidas para la gráfica de líneas");
                    }

                    ApprovedChart.Chart = new Microcharts.LineChart
                    {
                        Entries = approvedEntries,
                        BackgroundColor = SkiaSharp.SKColors.White,
                        LabelTextSize = 28,
                        LineMode = Microcharts.LineMode.Straight,
                        PointMode = Microcharts.PointMode.Circle,
                        PointSize = 15,
                        IsAnimated = true
                    };

                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ✅ Gráfica de líneas: {approvedEntries.Count} puntos");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ No se pudieron cargar datos para gráficas");
                    
                    // Mostrar gráficas vacías
                    var emptyEntry = new List<Microcharts.ChartEntry>
                    {
                        new Microcharts.ChartEntry(0) { Label = "Sin datos", ValueLabel = "0", Color = SkiaSharp.SKColor.Parse("#CCCCCC") }
                    };

                    AssignmentsChart.Chart = new Microcharts.BarChart { Entries = emptyEntry };
                    ApprovedChart.Chart = new Microcharts.LineChart { Entries = emptyEntry };
                }

                System.Diagnostics.Debug.WriteLine("[AdminDashboard] ✅ Gráficas cargadas correctamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ Error cargando gráficas: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] StackTrace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Obtiene una versión corta del estado para mostrar en las gráficas
        /// </summary>
        private string GetShortStatus(string status)
        {
            return status switch
            {
                "SIN_ASIGNAR" => "Sin Asig.",
                "ASIGNADO" => "Asignado",
                "APROBADO" => "Aprobado",
                "RECHAZADO" => "Rechazado",
                "EN_PROCESO" => "En Proc.",
                _ => status?.Length > 8 ? status.Substring(0, 8) : status ?? "N/A"
            };
        }

        /// <summary>
        /// Botón para recargar las estadísticas
        /// </summary>
        private async void OnReloadStatistics(object sender, EventArgs e)
        {
            try
            {
                LoadingStatusLabel.Text = "🔄 Recargando datos...";
                LoadingStatusLabel.TextColor = Colors.Orange;
                
                await LoadStatisticsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ Error al recargar: {ex.Message}");
                LoadingStatusLabel.Text = $"❌ Error al recargar: {ex.Message}";
                LoadingStatusLabel.TextColor = Colors.Red;
            }
        }
    }
}
