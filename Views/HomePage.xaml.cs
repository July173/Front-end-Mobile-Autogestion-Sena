using AutogestionSenaMaui.ViewModels;
using AutogestionSena.MAUI.Api.Services;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace AutogestionSenaMaui.Views
{
    /// <summary>
    /// HomePage - Página principal después del login
    /// Equivalente a Home.tsx en React
    /// 
    /// Funcionalidad:
    /// 1. Lee datos del usuario desde Preferences
    /// 2. Determina el dashboard apropiado según el rol
    /// 3. Carga dinámicamente el dashboard en el contenedor
    /// 4. Muestra MainLayout (TopBar + Dashboard + Footer)
    /// 
    /// Mapeo de roles (igual que React):
    /// 1 = Admin → AdminDashboardPage
    /// 2 = Aprendiz → ApprenticeDashboardPage
    /// 3 = Instructor → InstructorDashboardPage
    /// 4 = Coordinador → CoordinatorDashboardPage
    /// 5 = Operador SofiaPlus → SofiaOperatorDashboardPage
    /// </summary>
    public partial class HomePage : ContentPage
    {
        private readonly MainLayoutViewModel _viewModel;
        private int _userRole = 0;
        private int _apprenticeId = 0;
        private string _userName = "Usuario";

        public HomePage()
        {
            InitializeComponent();
            
            _viewModel = new MainLayoutViewModel();
            BindingContext = _viewModel;
            
            Debug.WriteLine("[HOME] HomePage initialized");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            Debug.WriteLine("[HOME] HomePage appearing");
            
            // Cargar datos del usuario y dashboard
            await LoadUserDataAndDashboard();
        }

        /// <summary>
        /// Carga los datos del usuario desde Preferences y el dashboard apropiado
        /// Similar a useEffect en Home.tsx
        /// </summary>
        private async Task LoadUserDataAndDashboard()
        {
            try
            {
                Debug.WriteLine("[HOME] Loading user data...");
                
                // Leer datos del usuario desde Preferences
                _userRole = Preferences.Get("UserRole", 0);
                var userEmail = Preferences.Get("UserEmail", string.Empty);
                
                Debug.WriteLine($"[HOME] User role: {_userRole}, Email: {userEmail}");

                // Si no hay rol, redirigir a login
                if (_userRole == 0)
                {
                    Debug.WriteLine("[HOME] No role found, redirecting to login");
                    await Shell.Current.GoToAsync("///LoginPage");
                    return;
                }

                // Obtener nombre del usuario desde el email
                _userName = GetUserNameFromEmail(userEmail);
                
                // Si el rol es Aprendiz, obtener apprentice_id
                if (_userRole == 2)
                {
                    await LoadApprenticeId();
                }

                // Actualizar breadcrumb según el rol
                UpdateBreadcrumbForRole(_userRole);

                // Cargar el dashboard apropiado
                await LoadDashboardForRole(_userRole);
                
                // Simular carga de notificaciones (opcional)
                await Task.Delay(500);
                _viewModel.UpdateNotificationCount(GetNotificationCountForRole(_userRole));
                
                Debug.WriteLine("[HOME] Dashboard loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HOME] Error loading user data: {ex.Message}");
                await DisplayAlert("Error", "Error al cargar el dashboard. Por favor, inicia sesión nuevamente.", "OK");
                await Shell.Current.GoToAsync("///LoginPage");
            }
        }

        /// <summary>
        /// Obtiene el apprentice_id para usuarios con rol Aprendiz
        /// Similar a fetchApprenticeId en Home.tsx
        /// </summary>
        private async Task LoadApprenticeId()
        {
            try
            {
                var userDataJson = Preferences.Get("user_data", string.Empty);
                if (!string.IsNullOrEmpty(userDataJson))
                {
                    var userData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(userDataJson);
                    
                    if (userData.TryGetProperty("person", out var personElement))
                    {
                        var personId = personElement.GetInt32();
                        
                        // Llamar al servicio para obtener apprentice_id
                        // TODO: Implementar ApprenticeService completo
                        // var apprenticeService = new ApprenticeService();
                        // var apprenticeData = await apprenticeService.GetApprenticeByPersonAsync(personId);
                        
                        // Temporal: usar personId como apprenticeId
                        _apprenticeId = personId;
                        Debug.WriteLine($"[HOME] Using Person ID as Apprentice ID: {_apprenticeId}");
                        
                        /* Código original comentado:
                        var apprenticeData = await apprenticeService.GetApprenticeByPersonAsync(personId);
                        
                        // if (apprenticeData != null && int.TryParse(apprenticeData.Id, out int apprenticeId))
                        // {
                        //     _apprenticeId = apprenticeId;
                        //     Debug.WriteLine($"[HOME] Apprentice ID loaded: {_apprenticeId}");
                        // }
                        */
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HOME] Error loading apprentice ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Carga el dashboard apropiado según el rol del usuario
        /// Similar al switch/case en Home.tsx
        /// </summary>
        private async Task LoadDashboardForRole(int roleId)
        {
            try
            {
                Debug.WriteLine($"[HOME] Loading dashboard for role: {roleId}");

                View? dashboardView = roleId switch
                {
                    1 => CreateAdminDashboard(),
                    2 => CreateApprenticeDashboard(),
                    3 => CreateInstructorDashboard(),
                    4 => CreateCoordinatorDashboard(),
                    5 => CreateSofiaOperatorDashboard(),
                    _ => CreateGenericDashboard()
                };

                if (dashboardView != null)
                {
                    // Ocultar loading y mostrar dashboard
                    LoadingView.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                    DashboardScrollView.IsVisible = true;
                    
                    // Inyectar el dashboard en el contenedor
                    DashboardContainer.Content = dashboardView;
                    Debug.WriteLine($"[HOME] Dashboard loaded for role {roleId}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HOME] Error loading dashboard: {ex.Message}");
                
                // Ocultar loading y mostrar error
                LoadingView.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                DashboardScrollView.IsVisible = true;
                
                DashboardContainer.Content = CreateErrorView(ex.Message);
            }
        }

        #region Dashboard Creation Methods

        /// <summary>
        /// Crea la vista del dashboard de administrador
        /// </summary>
        private View CreateAdminDashboard()
        {
            Debug.WriteLine("[HOME] Creating AdminDashboard view");
            var adminPage = new AdminDashboardPage();
            return adminPage.Content;
        }

        /// <summary>
        /// Crea la vista del dashboard de aprendiz
        /// </summary>
        private View CreateApprenticeDashboard()
        {
            Debug.WriteLine($"[HOME] Creating ApprenticeDashboard view (apprenticeId: {_apprenticeId})");
            var apprenticePage = new ApprenticeDashboardPage();
            // Aquí podrías pasar el apprenticeId al ViewModel si es necesario
            return apprenticePage.Content;
        }

        /// <summary>
        /// Crea la vista del dashboard de instructor
        /// </summary>
        private View CreateInstructorDashboard()
        {
            Debug.WriteLine("[HOME] Creating InstructorDashboard view");
            var instructorPage = new InstructorDashboardPage();
            return instructorPage.Content;
        }

        /// <summary>
        /// Crea la vista del dashboard de coordinador
        /// Similar al AdminDashboard según Home.tsx
        /// </summary>
        private View CreateCoordinatorDashboard()
        {
            Debug.WriteLine("[HOME] Creating CoordinatorDashboard view");
            var coordinatorPage = new CoordinatorDashboardPage();
            return coordinatorPage.Content;
        }

        /// <summary>
        /// Crea la vista del dashboard de operador SofiaPlus
        /// </summary>
        private View CreateSofiaOperatorDashboard()
        {
            Debug.WriteLine("[HOME] Creating SofiaOperatorDashboard view");
            var sofiaPage = new SofiaOperatorDashboardPage();
            return sofiaPage.Content;
        }

        /// <summary>
        /// Crea una vista genérica para roles no reconocidos
        /// Similar a GenericDashboardView en React
        /// </summary>
        private View CreateGenericDashboard()
        {
            Debug.WriteLine("[HOME] Creating Generic dashboard view");
            
            return new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Padding = 40,
                    Spacing = 24,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Frame
                        {
                            BackgroundColor = Color.FromArgb("#F5F5F5"),
                            CornerRadius = 12,
                            HasShadow = false,
                            Padding = 24,
                            Content = new VerticalStackLayout
                            {
                                Spacing = 12,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "¡ Bienvenido !",
                                        FontSize = 28,
                                        FontAttributes = FontAttributes.Bold,
                                        TextColor = Color.FromArgb("#39A900"),
                                        HorizontalOptions = LayoutOptions.Center
                                    },
                                    new Label
                                    {
                                        Text = _userName,
                                        FontSize = 20,
                                        TextColor = Color.FromArgb("#333333"),
                                        HorizontalOptions = LayoutOptions.Center
                                    }
                                }
                            }
                        },
                        new Frame
                        {
                            BackgroundColor = Colors.White,
                            CornerRadius = 8,
                            HasShadow = true,
                            Padding = 24,
                            Content = new VerticalStackLayout
                            {
                                Spacing = 8,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "No tienes una vista personalizada asignada",
                                        FontSize = 16,
                                        TextColor = Color.FromArgb("#666666"),
                                        HorizontalOptions = LayoutOptions.Center
                                    },
                                    new Label
                                    {
                                        Text = "Por favor contacta al administrador si necesitas acceso a funcionalidades especiales.",
                                        FontSize = 14,
                                        TextColor = Color.FromArgb("#999999"),
                                        HorizontalOptions = LayoutOptions.Center,
                                        HorizontalTextAlignment = TextAlignment.Center
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Crea una vista de error
        /// </summary>
        private View CreateErrorView(string errorMessage)
        {
            return new VerticalStackLayout
            {
                Padding = 40,
                Spacing = 16,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = "⚠️ Error al cargar el dashboard",
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromArgb("#DC395F"),
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = errorMessage,
                        FontSize = 14,
                        TextColor = Color.FromArgb("#666666"),
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    new Button
                    {
                        Text = "Reintentar",
                        BackgroundColor = Color.FromArgb("#39A900"),
                        TextColor = Colors.White,
                        CornerRadius = 8,
                        Padding = new Thickness(24, 12),
                        Command = new Command(async () => await LoadUserDataAndDashboard())
                    }
                }
            };
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Extrae el nombre del usuario desde el email
        /// Similar a getUserName en Home.tsx
        /// </summary>
        private string GetUserNameFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return "Usuario";

            try
            {
                var emailPart = email.Split('@')[0];
                var nameParts = emailPart.Split('.');
                
                if (nameParts.Length >= 2)
                {
                    // Capitalizar primera letra de cada parte
                    var firstName = char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1).ToLower();
                    var lastName = char.ToUpper(nameParts[1][0]) + nameParts[1].Substring(1).ToLower();
                    return $"{firstName} {lastName}";
                }
                else if (nameParts.Length == 1)
                {
                    return char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1).ToLower();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[HOME] Error extracting name from email: {ex.Message}");
            }

            return "Usuario";
        }

        /// <summary>
        /// Actualiza el breadcrumb según el rol
        /// </summary>
        private void UpdateBreadcrumbForRole(int roleId)
        {
            var moduleName = roleId switch
            {
                1 => "Administración",
                2 => "Aprendiz",
                3 => "Instructor",
                4 => "Coordinación",
                5 => "Operador SofiaPlus",
                _ => "General"
            };

            _viewModel.UpdateBreadcrumb(moduleName, "Dashboard");
        }

        /// <summary>
        /// Obtiene el número de notificaciones según el rol (simulado)
        /// En producción, esto debería venir de un servicio
        /// </summary>
        private int GetNotificationCountForRole(int roleId)
        {
            // TODO: Implementar servicio real de notificaciones
            return roleId switch
            {
                1 => 5,  // Admin suele tener más notificaciones
                2 => 2,  // Aprendiz
                3 => 3,  // Instructor
                4 => 4,  // Coordinador
                5 => 1,  // Operador
                _ => 0
            };
        }

        #endregion
    }
}
