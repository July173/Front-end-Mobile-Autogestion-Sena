using AutogestionSenaMaui.ViewModels;
namespace AutogestionSenaMaui.ContentViews;

public partial class DashboardLayout : ContentView
{
    private bool _isMenuOpen = false;

    public DashboardLayout()
    {
        InitializeComponent();
        // Cuando el componente se haya cargado, intentar seleccionar el dashboard por defecto
        this.Loaded += DashboardLayout_Loaded;
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

    private async void DashboardLayout_Loaded(object? sender, EventArgs e)
    {
        // Si SideMenu está inicializado, esperar su carga y seleccionar la ruta por defecto
        try
        {
            var vm = SideMenu?.BindingContext as AutogestionSenaMaui.ViewModels.DynamicSideMenuViewModel;
            if (vm == null) return;

            // Esperar hasta que el menu termine de cargar (con timeout)
            var timeout = DateTime.UtcNow.AddSeconds(8);
            while (vm.IsLoading && DateTime.UtcNow < timeout)
            {
                await Task.Delay(100);
            }

            // Encontrar ruta por defecto: 'Inicio' o 'MainDashboard' o la primera disponible (incluso submenus)
            string? route = null;
            MenuItemViewModel? inicioItem = null;

            // Revisa elementos principales (por nombre, por mapeo de ruta o por backend route '/home')
            inicioItem = vm.MenuItems.FirstOrDefault(m =>
                (!string.IsNullOrEmpty(m.Route) && (m.Name?.ToLower().Contains("inicio") == true || (m.Route?.ToLower().Contains("main") == true) || (m.Route?.ToLower().Contains("dashboard") == true))))
                ?? vm.MenuItems.FirstOrDefault(m => (m.BackendRoute?.ToLower().Contains("/home") == true || m.BackendRoute?.ToLower().Contains("home") == true));

            // Si no, revisa submenus
            if (inicioItem == null)
            {
                foreach (var m in vm.MenuItems)
                {
                    var sub = m.SubMenus.FirstOrDefault(s => !string.IsNullOrEmpty(s.Route) && (s.Name?.ToLower().Contains("inicio") == true || s.Route?.ToLower().Contains("main") == true || s.Route?.ToLower().Contains("dashboard") == true)
                        || (s.BackendRoute?.ToLower().Contains("/home") == true || s.BackendRoute?.ToLower().Contains("home") == true));
                    if (sub != null)
                    {
                        inicioItem = sub;
                        break;
                    }
                }
            }

            // Si aun no encontramos, tomar la primera ruta disponible (item o submenu)
            if (inicioItem == null)
            {
                if (vm.MenuItems.Any(m => !string.IsNullOrEmpty(m.Route)))
                    inicioItem = vm.MenuItems.First(m => !string.IsNullOrEmpty(m.Route));
                else
                {
                    var s = vm.MenuItems.SelectMany(m => m.SubMenus).FirstOrDefault(sm => !string.IsNullOrEmpty(sm.Route));
                    if (s != null) inicioItem = s;
                }
            }

            if (inicioItem != null)
                route = inicioItem.Route;

            if (!string.IsNullOrEmpty(route))
            {
                // Ejecutar la navegación usando el comando del viewmodel si está disponible
                if (vm.NavigateCommand?.CanExecute(route) == true)
                    vm.NavigateCommand.Execute(route);
                else if (Shell.Current != null)
                    await Shell.Current.GoToAsync($"//{route}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DashboardLayout] Error al seleccionar ruta por defecto: {ex}");
        }
    }
}
