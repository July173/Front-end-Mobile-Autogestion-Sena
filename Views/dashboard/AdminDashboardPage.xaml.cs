using System;
using System.Linq;
using AutogestionSenaMaui.ViewModels;
using AutogestionSena.MAUI.Api.Services;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Views
{
    public partial class AdminDashboardPage : ContentPage
    {
        private readonly AdminDashboardViewModel _viewModel;
        private readonly MenuService _menuService;

        public AdminDashboardPage()
        {
            InitializeComponent();
            _viewModel = new AdminDashboardViewModel();
            _menuService = new MenuService();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await _viewModel.LoadDataAsync();
                await LoadDebugDataAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] Error cargando datos: {ex}");
            }
        }

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

                // Mostrar datos del usuario
                UserIdLabel.Text = $"{(userId > 0 ? "✅" : "❌")} UserId: {userId}";
                UserRoleLabel.Text = $"{(userRole > 0 ? "✅" : "❌")} UserRole: {userRole} ({GetRoleName(userRole)})";
                UserEmailLabel.Text = $"{(string.IsNullOrEmpty(userEmail) ? "❌" : "✅")} Email: {(string.IsNullOrEmpty(userEmail) ? "N/A" : userEmail)}";
                UserNameLabel.Text = $"✅ Nombre: {GetUserNameFromEmail(userEmail)}";
                AuthTokenLabel.Text = $"{(string.IsNullOrEmpty(authToken) ? "❌" : "✅")} Token: {(string.IsNullOrEmpty(authToken) ? "No disponible" : authToken.Substring(0, Math.Min(20, authToken.Length)) + "...")}";

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
                    MenuStatusLabel.Text = $"⏳ Cargando menú desde API (ID={idParaMenu})...";
                    MenuStatusLabel.TextColor = Colors.Orange;

                    try
                    {
                        var menuData = await _menuService.GetMenuItemsAsync(idParaMenu.ToString());

                        if (menuData != null && menuData.MenuItems != null && menuData.MenuItems.Any())
                        {
                            MenuStatusLabel.Text = $"✅ Menú cargado exitosamente - {menuData.MenuItems.Count} módulos encontrados";
                            MenuStatusLabel.TextColor = Colors.Green;

                            // Limpiar contenedor
                            MenuItemsContainer.Children.Clear();

                            // Mostrar información del rol
                            MenuItemsContainer.Children.Add(new Label
                            {
                                Text = $"👤 Rol del Usuario: {menuData.UserInfo.Role}",
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Color.FromArgb("#0D6EFD"),
                                FontSize = 15,
                                Margin = new Thickness(0, 8, 0, 8)
                            });

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
                                MenuItemsContainer.Children.Add(moduleFrame);
                            }

                            System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ✅ Menú mostrado: {menuData.MenuItems.Count} módulos");
                        }
                        else
                        {
                            MenuStatusLabel.Text = "⚠️ No se encontraron módulos en el menú";
                            MenuStatusLabel.TextColor = Colors.Orange;
                            System.Diagnostics.Debug.WriteLine("[AdminDashboard] ⚠️ Menú vacío");
                        }
                    }
                    catch (Exception ex)
                    {
                        MenuStatusLabel.Text = $"❌ Error al cargar menú: {ex.Message}";
                        MenuStatusLabel.TextColor = Colors.Red;
                        System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ Error cargando menú: {ex}");

                        MenuItemsContainer.Children.Clear();
                        MenuItemsContainer.Children.Add(new Label
                        {
                            Text = $"Error detallado:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}",
                            TextColor = Colors.Red,
                            FontSize = 12
                        });
                    }
                }
                else
                {
                    MenuStatusLabel.Text = $"❌ No se encontró ningún ID válido (UserId={userId}, UserPerson={Preferences.Get("UserPerson", 0)}, UserRole={userRole})";
                    MenuStatusLabel.TextColor = Colors.Red;
                    System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ No se puede cargar menú - UserId={userId}, UserPerson={Preferences.Get("UserPerson", 0)}, UserRole={userRole}");
                    
                    MenuItemsContainer.Children.Clear();
                    MenuItemsContainer.Children.Add(new Label
                    {
                        Text = "⚠️ PROBLEMA: El API de login no está retornando el UserId.\n\nPor favor verifica:\n1. La respuesta del endpoint de login\n2. Que response.User.Id tenga un valor > 0\n3. Los logs en la consola",
                        TextColor = Colors.Red,
                        FontSize = 13,
                        Padding = 12
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminDashboard] ❌ Error en LoadDebugDataAsync: {ex}");
            }
        }

        /// <summary>
        /// Botón para recargar datos de debug
        /// </summary>
        private async void OnReloadDebugData(object sender, EventArgs e)
        {
            await LoadDebugDataAsync();
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
    }
}
