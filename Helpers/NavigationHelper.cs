using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Helpers
{
    /// <summary>
    /// Helper para manejar navegación protegida y redirección basada en roles.
    /// Similar a ProtectedRoute de React.
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Verifica si el usuario está autenticado
        /// </summary>
        public static bool IsUserAuthenticated()
        {
            var token = Preferences.Get("AuthToken", string.Empty);
            return !string.IsNullOrEmpty(token);
        }

        /// <summary>
        /// Obtiene el rol del usuario actual
        /// </summary>
        public static int GetUserRole()
        {
            return Preferences.Get("UserRole", 0);
        }

        /// <summary>
        /// Obtiene la ruta del dashboard según el rol del usuario
        /// Ahora todos los roles navegan a HomePage, que carga el dashboard dinámicamente
        /// </summary>
        public static string GetDashboardRouteForRole(int roleId)
        {
            // Con la nueva arquitectura, todos navegan a HomePage
            // HomePage se encarga de cargar el dashboard apropiado según el rol
            return "HomePage";
        }

        /// <summary>
        /// Obtiene el nombre del rol
        /// </summary>
        public static string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Administrador",
                2 => "Aprendiz",
                3 => "Instructor",
                4 => "Coordinador",
                5 => "Operador SofiaPlus",
                _ => "Usuario"
            };
        }

        /// <summary>
        /// Verifica si el usuario tiene permiso para acceder a una ruta específica
        /// Con la arquitectura simplificada: LoginPage (público) y HomePage (protegido)
        /// </summary>
        public static bool CanAccessRoute(string route, int userRole)
        {
            // Rutas públicas (siempre accesibles sin autenticación)
            var publicRoutes = new[]
            {
                "LoginPage",
                "RegisterPage",
                "PasswordRecoveryPage",
                "CodeVerificationPage",
                "PasswordResetPage"
            };

            if (Array.Exists(publicRoutes, r => r.Equals(route, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            // Si no está autenticado, no puede acceder a rutas protegidas
            if (!IsUserAuthenticated())
            {
                return false;
            }

            // HomePage es accesible para todos los usuarios autenticados
            // HomePage se encarga internamente de cargar el dashboard apropiado según el rol
            if (route.Equals("HomePage", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // SecurityMainPage es accesible para usuarios autenticados
            // La página puede validar permisos específicos internamente
            if (route.Equals("SecurityMainPage", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Por defecto, denegar acceso a rutas no definidas
            return false;
        }

        /// <summary>
        /// Navega a la ruta con validación de permisos
        /// </summary>
        public static async Task<bool> NavigateToAsync(string route, bool animate = true)
        {
            try
            {
                // Verificar autenticación
                if (!IsUserAuthenticated())
                {
                    System.Diagnostics.Debug.WriteLine($"[NAV] Usuario no autenticado, redirigiendo a Login");
                    await Shell.Current.GoToAsync("///LoginPage", animate);
                    return false;
                }

                var userRole = GetUserRole();

                // Verificar permisos
                if (!CanAccessRoute(route, userRole))
                {
                    System.Diagnostics.Debug.WriteLine($"[NAV] Usuario rol={userRole} no tiene permiso para acceder a {route}");
                    
                    // Mostrar mensaje de error
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Acceso Denegado",
                            "No tienes permisos para acceder a esta sección.",
                            "Aceptar"
                        );
                    }

                    // Redirigir al dashboard correspondiente
                    var dashboardRoute = GetDashboardRouteForRole(userRole);
                    await Shell.Current.GoToAsync($"///{dashboardRoute}", animate);
                    return false;
                }

                // Navegar a la ruta solicitada
                System.Diagnostics.Debug.WriteLine($"[NAV] Navegando a {route}");
                await Shell.Current.GoToAsync($"///{route}", animate);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NAV] Error en NavigateToAsync: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Navega al dashboard correspondiente según el rol del usuario actual
        /// </summary>
        public static async Task NavigateToDashboardAsync(bool animate = true)
        {
            var userRole = GetUserRole();
            var dashboardRoute = GetDashboardRouteForRole(userRole);
            await NavigateToAsync(dashboardRoute, animate);
        }

        /// <summary>
        /// Cierra sesión y redirige al login
        /// </summary>
        public static async Task LogoutAsync()
        {
            try
            {
                // Limpiar datos de autenticación
                Preferences.Remove("AuthToken");
                Preferences.Remove("RefreshToken");
                Preferences.Remove("UserRole");
                Preferences.Remove("user_data");
                Preferences.Remove("UserId");

                // Limpiar SecureStorage
                try
                {
                    SecureStorage.Remove("user_data");
                    SecureStorage.Remove("AuthToken");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[NAV] Error limpiando SecureStorage: {ex}");
                }

                System.Diagnostics.Debug.WriteLine("[NAV] Sesión cerrada, redirigiendo a Login");

                // Redirigir a login
                if (Shell.Current != null)
                {
                    await Shell.Current.GoToAsync("///LoginPage", true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NAV] Error en LogoutAsync: {ex}");
            }
        }

        /// <summary>
        /// Valida la sesión actual y redirige si es necesario
        /// </summary>
        public static async Task ValidateSessionAsync()
        {
            if (!IsUserAuthenticated())
            {
                System.Diagnostics.Debug.WriteLine("[NAV] Sesión no válida, redirigiendo a Login");
                await Shell.Current.GoToAsync("///LoginPage", true);
            }
        }
    }
}
