namespace AutogestionSenaMaui.ContentViews;

public partial class DashboardLayout : ContentView
{
    private bool _isMenuOpen = false;

    public DashboardLayout()
    {
        InitializeComponent();
    }

    // Propiedad para establecer el texto del breadcrumb actual
    public string CurrentPage
    {
        get => BreadcrumbCurrent.Text;
        set => BreadcrumbCurrent.Text = value;
    }

    // Propiedad para establecer el contenido principal
    public View PageContent
    {
        get => MainContent.Content;
        set => MainContent.Content = value;
    }

    private async void OnMenuButtonClicked(object sender, EventArgs e)
    {
        if (_isMenuOpen)
        {
            await CloseMenu();
        }
        else
        {
            await OpenMenu();
        }
    }

    private async Task OpenMenu()
    {
        _isMenuOpen = true;
        
        // Mostrar overlay
        MenuOverlay.IsVisible = true;
        MenuOverlay.InputTransparent = false;
        
        // Animar menú y overlay
        var menuTask = SideMenu.TranslateTo(0, 0, 250, Easing.CubicOut);
        var overlayTask = MenuOverlay.FadeTo(0.5, 250);
        
        await Task.WhenAll(menuTask, overlayTask);
    }

    private async Task CloseMenu()
    {
        _isMenuOpen = false;
        
        // Animar menú y overlay
        var menuTask = SideMenu.TranslateTo(-320, 0, 250, Easing.CubicIn);
        var overlayTask = MenuOverlay.FadeTo(0, 250);
        
        await Task.WhenAll(menuTask, overlayTask);
        
        // Ocultar overlay
        MenuOverlay.IsVisible = false;
        MenuOverlay.InputTransparent = true;
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        await CloseMenu();
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

    // Método público para cerrar el menú desde fuera
    public async Task CloseMenuAsync()
    {
        if (_isMenuOpen)
        {
            await CloseMenu();
        }
    }
}
