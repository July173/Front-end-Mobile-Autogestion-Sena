using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
namespace AutogestionSenaMaui.ContentViews;

public partial class DashboardLayout : ContentView
{
    public DashboardLayout()
    {
        InitializeComponent();
        // Cuando el componente se haya cargado, intentar seleccionar el dashboard por defecto
        this.Loaded += DashboardLayout_Loaded;
    }

    // Propiedad para establecer el contenido principal
    public View PageContent
    {
        get => MainContent.Content;
        set => MainContent.Content = value;
    }

    private async void OnNotificationsTapped(object sender, EventArgs e)
    {
        try
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//notifications");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[NAV] Error navegando a notificaciones: {ex}");
        }
    }

    private void DashboardLayout_Loaded(object? sender, EventArgs e)
    {
        // El menú lateral ahora es manejado por las páginas principales (MainLayoutPage/HomePage)
        // Este método se mantiene para compatibilidad pero ya no gestiona el menú
    }
}
