using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.ContentViews
{
    /// <summary>
    /// TopBar - Barra superior de navegación
    /// Frame 425: Muestra breadcrumb navigation y notificaciones
    /// Similar al header en MainLayout.tsx de React
    /// </summary>
    public partial class TopBar : ContentView
    {
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
        }

        // Evento público para notificar clicks del botón de menú (TopBar)
        public event EventHandler? MenuButtonClicked;

        // Lógica para manejar el click del menú desde XAML y delegar la acción
        private void OnMenuButtonClicked(object sender, EventArgs e)
        {
            // Si hay suscriptores, notificar; de lo contrario, fallback a Shell.Flyout
            if (MenuButtonClicked != null)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("[TopBar] OnMenuButtonClicked - invoking MenuButtonClicked");
                    MenuButtonClicked?.Invoke(this, EventArgs.Empty);
                    return;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[TopBar] Error al invocar MenuButtonClicked: {ex}");
                }
            }

            // Fallback: si no hay suscriptores o hubo fallo, abrir el Flyout del Shell por compatibilidad
            var mainPage = Application.Current?.MainPage;
            if (mainPage is Shell shell)
            {
                System.Diagnostics.Debug.WriteLine("[TopBar] No subscribers for MenuButtonClicked - opening Shell.FlyoutIsPresented as fallback");
                shell.FlyoutIsPresented = true;
            }
        }

        // Handler para notificaciones (XAML Tapped)
        private async void OnNotificationsTapped(object sender, EventArgs e)
        {
            if (Shell.Current != null)
            {
                try
                {
                    await Shell.Current.GoToAsync("//notifications");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[TopBar] Error navegando a notificaciones: {ex}");
                }
            }
        }
    }
}
