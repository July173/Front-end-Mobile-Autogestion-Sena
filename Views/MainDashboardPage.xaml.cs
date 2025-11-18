namespace AutogestionSenaMaui.Views;

public partial class MainDashboardPage : ContentPage
{
    public MainDashboardPage()
    {
        InitializeComponent();
    }

    private void OnMenuButtonClicked(object sender, EventArgs e)
    {
        // Mostrar/ocultar menú lateral en móviles
        // Implementar lógica de toggle del menú
    }

    private async void OnNotificationsTapped(object sender, EventArgs e)
    {
        // Navegar a notificaciones
        try
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//notifications");
            }
            else if (Navigation != null)
            {
                // Si no hay Shell, intentar PushAsync a la página de notificaciones (si existe)
                // Nota: Reemplazar 'NotificationsPage' por el page concreto si es necesario
                await Navigation.PushAsync(new MainDashboardPage());
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[NAV] Error navegando a notificaciones: {ex}");
        }
    }
}
