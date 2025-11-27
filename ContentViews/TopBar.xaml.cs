using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.Controls;
using AutogestionSenaMaui.Helpers;
using System.Timers;
using AutogestionSena.MAUI.Views.notificaciones;

namespace AutogestionSenaMaui.ContentViews
{
    /// <summary>
    /// TopBar - Barra superior de navegación responsive con overlay modal
    /// Frame 425: Muestra breadcrumb navigation y menú desplegable flotante
    /// Adaptable para pantallas pequeñas y grandes con overlay completo
    /// </summary>
    public partial class TopBar : ContentView
    {
        private bool _isMenuOpen = false;
        private System.Timers.Timer? _autoHideTimer;
        private const int AUTO_HIDE_DELAY = 7000; // 7 segundos para overlay

        // ARMAR ACCESORES: Exponer el texto del breadcrumb sin crear conflictos con el members auto-generado
        public string BreadcrumbRootText
        {
            get => this.FindByName<Label>("BreadcrumbRoot")?.Text ?? string.Empty;
            set { var l = this.FindByName<Label>("BreadcrumbRoot"); if (l != null) l.Text = value; }
        }

        public string BreadcrumbCurrentText
        {
            get => this.FindByName<Label>("BreadcrumbCurrent")?.Text ?? string.Empty;
            set { var l = this.FindByName<Label>("BreadcrumbCurrent"); if (l != null) l.Text = value; }
        }

        public TopBar()
        {
            InitializeComponent();
            InitializeAutoHideTimer();
        }

        // Evento público para notificar clicks del botón de menú (TopBar)
        public event EventHandler? MenuButtonClicked;

        // Inicializar timer para auto-ocultar el menú
        private void InitializeAutoHideTimer()
        {
            _autoHideTimer = new System.Timers.Timer(AUTO_HIDE_DELAY);
            _autoHideTimer.Elapsed += OnAutoHideTimer;
            _autoHideTimer.AutoReset = false; // Solo se ejecuta una vez por inicio
        }

        // Override para detectar cambios de tamaño y aplicar diseño responsive
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            ApplyResponsiveLayout(width);
        }

        // Aplicar diseño responsive según el ancho de la pantalla
        private void ApplyResponsiveLayout(double width)
        {
            try
            {
                // El overlay modal funciona bien en todas las pantallas
                System.Diagnostics.Debug.WriteLine($"[TopBar] Ancho de pantalla: {width}px - Usando overlay modal");
                
                // Ajustar el ancho del menú según la pantalla
                var dropdownMenu = this.FindByName<Border>("DropdownMenu");
                if (dropdownMenu != null)
                {
                    if (width < 400)
                    {
                        dropdownMenu.WidthRequest = width * 0.85; // 85% del ancho en pantallas pequeñas
                        dropdownMenu.Margin = new Thickness(10, 80, 10, 20);
                    }
                    else if (width < 600)
                    {
                        dropdownMenu.WidthRequest = 300;
                        dropdownMenu.Margin = new Thickness(15, 80, 15, 20);
                    }
                    else
                    {
                        dropdownMenu.WidthRequest = 320;
                        dropdownMenu.Margin = new Thickness(20, 80, 20, 20);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error aplicando diseño responsive: {ex}");
            }
        }

        // Handler para el botón hamburguesa
        private async void OnHamburgerTapped(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Hamburger tapped. Menu currently: {(_isMenuOpen ? "open" : "closed")}");

                if (_isMenuOpen)
                {
                    await HideOverlayMenu();
                }
                else
                {
                    await ShowOverlayMenu();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error en OnHamburgerTapped: {ex}");
            }
        }

        // Handler para tap en el overlay (fondo) - oculta el menú
        private async void OnOverlayTapped(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[TopBar] Overlay background tapped, hiding menu...");
                if (_isMenuOpen)
                {
                    await HideOverlayMenu();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error en OnOverlayTapped: {ex}");
            }
        }

        // Mostrar el overlay modal con animación
        private async Task ShowOverlayMenu()
        {
            try
            {
                var overlayContainer = this.FindByName<Grid>("OverlayContainer");
                var dropdownMenu = this.FindByName<Border>("DropdownMenu");
                
                if (overlayContainer == null || dropdownMenu == null) return;

                System.Diagnostics.Debug.WriteLine("[TopBar] Showing overlay menu...");

                // Preparar el overlay para la animación
                overlayContainer.IsVisible = true;
                overlayContainer.Opacity = 0;
                
                // Preparar el menú para la animación (viene desde arriba)
                dropdownMenu.Opacity = 0;
                dropdownMenu.TranslationY = -100;
                dropdownMenu.Scale = 0.9;

                // Animar el overlay (fondo)
                var overlayFadeIn = overlayContainer.FadeTo(1, 200, Easing.CubicOut);
                
                // Animar el menú (deslizar desde arriba + fade + scale)
                var menuFadeIn = dropdownMenu.FadeTo(1, 300, Easing.CubicOut);
                var menuSlideIn = dropdownMenu.TranslateTo(0, 0, 300, Easing.CubicOut);
                var menuScaleIn = dropdownMenu.ScaleTo(1, 300, Easing.CubicOut);

                await Task.WhenAll(overlayFadeIn);
                await Task.WhenAll(menuFadeIn, menuSlideIn, menuScaleIn);

                _isMenuOpen = true;

                // Iniciar el timer para auto-ocultar
                StartAutoHideTimer();

                System.Diagnostics.Debug.WriteLine("[TopBar] Overlay menu shown successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error showing overlay menu: {ex}");
            }
        }

        // Ocultar el overlay modal con animación
        private async Task HideOverlayMenu()
        {
            try
            {
                var overlayContainer = this.FindByName<Grid>("OverlayContainer");
                var dropdownMenu = this.FindByName<Border>("DropdownMenu");
                
                if (overlayContainer == null || !overlayContainer.IsVisible || dropdownMenu == null) return;

                System.Diagnostics.Debug.WriteLine("[TopBar] Hiding overlay menu...");

                // Parar el timer
                StopAutoHideTimer();

                // Animar el menú (deslizar hacia arriba + fade + scale)
                var menuFadeOut = dropdownMenu.FadeTo(0, 200, Easing.CubicIn);
                var menuSlideOut = dropdownMenu.TranslateTo(0, -50, 200, Easing.CubicIn);
                var menuScaleOut = dropdownMenu.ScaleTo(0.9, 200, Easing.CubicIn);

                await Task.WhenAll(menuFadeOut, menuSlideOut, menuScaleOut);

                // Animar el overlay (fondo)
                await overlayContainer.FadeTo(0, 150, Easing.CubicIn);

                overlayContainer.IsVisible = false;
                _isMenuOpen = false;

                System.Diagnostics.Debug.WriteLine("[TopBar] Overlay menu hidden successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error hiding overlay menu: {ex}");
            }
        }

        // Iniciar el timer para auto-ocultar (tiempo extendido para overlay)
        private void StartAutoHideTimer()
        {
            try
            {
                _autoHideTimer?.Stop();
                _autoHideTimer?.Start();
                System.Diagnostics.Debug.WriteLine($"[TopBar] Auto-hide timer started ({AUTO_HIDE_DELAY}ms)");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error starting auto-hide timer: {ex}");
            }
        }

        // Parar el timer
        private void StopAutoHideTimer()
        {
            try
            {
                _autoHideTimer?.Stop();
                System.Diagnostics.Debug.WriteLine("[TopBar] Auto-hide timer stopped");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error stopping auto-hide timer: {ex}");
            }
        }

        // Handler para el timer de auto-ocultar
        private async void OnAutoHideTimer(object? sender, ElapsedEventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[TopBar] Auto-hide timer elapsed, hiding overlay menu...");

                // Ejecutar en el hilo principal
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (_isMenuOpen)
                    {
                        await HideOverlayMenu();
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error en auto-hide timer: {ex}");
            }
        }

        // Handler para notificaciones - oculta el menú y navega
        private async void OnNotificationsTapped(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[TopBar] Notifications tapped");

                // Ocultar el menú primero
                if (_isMenuOpen)
                {
                    await HideOverlayMenu();
                }

                // Breve delay para que la animación se complete
                await Task.Delay(100);

                // Intentar navegar usando Shell route registrada
                try
                {
                    if (Shell.Current != null)
                    {
                        System.Diagnostics.Debug.WriteLine("[TopBar] Intentando Shell navigation to 'notifications'");
                        await Shell.Current.GoToAsync("notifications");
                        return;
                    }
                }
                catch (Exception exNav)
                {
                    System.Diagnostics.Debug.WriteLine($"[TopBar] Shell navigation failed: {exNav.Message}");
                }

                // Fallback: push page directamente en la pila de navegación
                try
                {
                    var page = new NotificacionesApp();
                    if (Application.Current?.MainPage is Shell shell && shell.CurrentPage != null)
                    {
                        await shell.CurrentPage.Navigation.PushAsync(page);
                    }
                    else if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(page);
                    }
                }
                catch (Exception exPush)
                {
                    System.Diagnostics.Debug.WriteLine($"[TopBar] Push navigation failed: {exPush}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error navegando a notificaciones: {ex}");
            }
        }

        // Handler para cerrar sesión - oculta el menú y muestra confirmación
        private async void OnLogoutTapped(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[TopBar] Logout tapped");

                // Ocultar el menú primero
                if (_isMenuOpen)
                {
                    await HideOverlayMenu();
                }

                // Breve delay para que la animación se complete
                await Task.Delay(200);

                // Mostrar diálogo de confirmación
                bool confirmLogout = await Application.Current.MainPage.DisplayAlert(
                    "Cerrar Sesión",
                    "¿Estás seguro que deseas cerrar sesión?",
                    "Sí, cerrar sesión",
                    "Cancelar");

                if (confirmLogout)
                {
                    await PerformLogout();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TopBar] Error en OnLogoutTapped: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al cerrar sesión.", "Aceptar");
            }
        }

        // Realiza el proceso completo de logout
        private async Task PerformLogout()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[LOGOUT] Iniciando proceso de cierre de sesión...");

                // 1. Limpiar todos los datos almacenados en Preferences
                ClearPreferencesData();

                // 2. Limpiar datos de SecureStorage
                await ClearSecureStorageData();

                // 3. Notificar a otros componentes del logout (si existe AuthEvents)
                try
                {
                    AuthEvents.NotifyUserLoggedOut();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[LOGOUT] Error notificando logout: {ex}");
                }

                // 4. Navegar al login
                System.Diagnostics.Debug.WriteLine("[LOGOUT] Redirigiendo al login...");
                await Shell.Current.GoToAsync("///LoginPage");

                System.Diagnostics.Debug.WriteLine("[LOGOUT] Proceso de cierre de sesión completado exitosamente");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGOUT] Error durante PerformLogout: {ex}");
                throw;
            }
        }

        // Limpia todos los datos de Preferences
        private void ClearPreferencesData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[LOGOUT] Limpiando datos de Preferences...");

                // Lista de todas las keys que se usan en la aplicación
                string[] preferencesKeys = {
                    "AuthToken",
                    "RefreshToken", 
                    "user_data",
                    "UserRole",
                    "password_reset_data",
                    "IsLoggedIn"
                };

                foreach (string key in preferencesKeys)
                {
                    if (Preferences.ContainsKey(key))
                    {
                        Preferences.Remove(key);
                        System.Diagnostics.Debug.WriteLine($"[LOGOUT] Eliminada key de Preferences: {key}");
                    }
                }

                // Limpiar todas las preferences (método alternativo más seguro)
                Preferences.Clear();
                System.Diagnostics.Debug.WriteLine("[LOGOUT] Todas las Preferences limpiadas");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGOUT] Error limpiando Preferences: {ex}");
            }
        }

        // Limpia todos los datos de SecureStorage
        private async Task ClearSecureStorageData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[LOGOUT] Limpiando datos de SecureStorage...");

                // Lista de todas las keys que se usan en SecureStorage
                string[] secureStorageKeys = {
                    "user_data",
                    "AuthToken",
                    "RefreshToken"
                };

                foreach (string key in secureStorageKeys)
                {
                    try
                    {
                        SecureStorage.Remove(key);
                        System.Diagnostics.Debug.WriteLine($"[LOGOUT] Eliminada key de SecureStorage: {key}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[LOGOUT] Error eliminando key {key} de SecureStorage: {ex}");
                    }
                }

                // Limpiar todo el SecureStorage (método alternativo)
                SecureStorage.RemoveAll();
                System.Diagnostics.Debug.WriteLine("[LOGOUT] Todos los datos de SecureStorage limpiados");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGOUT] Error limpiando SecureStorage: {ex}");
            }
        }

        // Dispose para limpiar el timer
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            
            if (Handler == null)
            {
                // El control se está destruyendo, limpiar el timer
                _autoHideTimer?.Dispose();
                _autoHideTimer = null;
                System.Diagnostics.Debug.WriteLine("[TopBar] Timer disposed on handler change");
            }
        }
    }
}
