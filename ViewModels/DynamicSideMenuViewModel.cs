using AutogestionSenaMaui.Api.Dtos;
using AutogestionSena.MAUI.Api.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutogestionSenaMaui.ViewModels;

/// <summary>
/// ViewModel para un item individual del menú
/// </summary>
public class MenuItemViewModel : INotifyPropertyChanged
{
    private bool _isExpanded;
    private bool _isSelected;

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string BackendRoute { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int Order { get; set; }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            _isExpanded = value;
            OnPropertyChanged();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<MenuItemViewModel> SubMenus { get; set; } = new();

    public ICommand ToggleCommand { get; set; } = new Command(() => { });
    public ICommand NavigateCommand { get; set; } = new Command(() => { });

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/// <summary>
/// ViewModel principal para el menú lateral dinámico
/// </summary>
public class DynamicSideMenuViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _userName = string.Empty;
    private string _userInitials = string.Empty;
    private string _roleName = string.Empty;
    private int _roleId;
    private bool _isLoading;

    public DynamicSideMenuViewModel()
    {
        _apiService = new ApiService();
        MenuItems = new ObservableCollection<MenuItemViewModel>();
        
        NavigateCommand = new Command<string>(OnNavigate);
        OpenProfileCommand = new Command(OnOpenProfile);
        
        // Cargar datos del usuario y menú
        Task.Run(async () => await InitializeAsync());
    }

    #region Properties

    public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
            UpdateUserInitials();
        }
    }

    public string UserInitials
    {
        get => _userInitials;
        set
        {
            _userInitials = value;
            OnPropertyChanged();
        }
    }

    public string RoleName
    {
        get => _roleName;
        set
        {
            _roleName = value;
            OnPropertyChanged();
        }
    }

    public int RoleId
    {
        get => _roleId;
        set
        {
            _roleId = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand NavigateCommand { get; }
    public ICommand OpenProfileCommand { get; }

    #endregion

    #region Methods

    private async Task InitializeAsync()
    {
            try
        {
            IsLoading = true;

            // Obtener datos del usuario desde SecureStorage
            var userJson = await SecureStorage.GetAsync("user_data");
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(userJson);
                UserName = user.GetProperty("firstName").GetString() ?? "Usuario";
                RoleId = user.GetProperty("roleId").GetInt32();
            }

            // Cargar menú según el rol
            await LoadMenuAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al inicializar menú: {ex.Message}");
            // Cargar menú por defecto en caso de error
            LoadDefaultMenu();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadMenuAsync()
    {
        try
        {
            // Llamar al endpoint del backend
            var menuResponse = await _apiService.GetAsync<MenuResponseDto>(
                $"security/rol-form-permissions/{RoleId}/get-menu");

            // If the MenuResponseDto is empty or null, try to parse alternative format (RoleModuleFormDto)
            if (menuResponse == null || menuResponse.MenuItems == null || !menuResponse.MenuItems.Any())
            {
                try
                {
                    var roleModuleForms = await _apiService.GetAsync<List<RoleModuleFormDto>>(
                        $"security/rol-form-permissions/{RoleId}/get-menu");

                    if (roleModuleForms != null && roleModuleForms.Any())
                    {
                        // Convert to MenuResponseDto
                        menuResponse = new MenuResponseDto
                        {
                            RoleId = this.RoleId,
                            RoleName = roleModuleForms.First().Rol ?? this.RoleName,
                            MenuItems = new List<MenuDto>()
                        };

                        int idCounter = 1000; // start id for generated items
                        foreach (var rm in roleModuleForms)
                        {
                            if (rm.ModuleForm == null) continue;
                            foreach (var mod in rm.ModuleForm)
                            {
                                var parent = new MenuDto
                                {
                                    Id = idCounter++,
                                    Name = mod.Name ?? "Module",
                                    Icon = "",
                                    Route = mod.Form != null && mod.Form.Count > 0 ? mod.Form[0].Path ?? string.Empty : string.Empty,
                                    Order = 0,
                                    SubMenus = new List<MenuDto>()
                                };

                                if (mod.Form != null)
                                {
                                    int subId = 1;
                                    foreach (var f in mod.Form)
                                    {
                                        parent.SubMenus.Add(new MenuDto
                                        {
                                            Id = idCounter++,
                                            Name = f.Name ?? "Form",
                                            Icon = "",
                                            Route = f.Path ?? string.Empty,
                                            ParentId = parent.Id,
                                            Order = subId++
                                        });
                                    }
                                }

                                menuResponse.MenuItems.Add(parent);
                            }
                        }
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing RoleModuleForm: {ex2}");
                }
            }

            if (menuResponse != null)
            {
                RoleName = menuResponse.RoleName;
                
                // Limpiar y cargar nuevo menú
                MenuItems.Clear();
                
                foreach (var menuItem in (menuResponse.MenuItems ?? Enumerable.Empty<MenuDto>()).OrderBy(m => m.Order))
                {
                    var menuItemVm = CreateMenuItemViewModel(menuItem);
                    MenuItems.Add(menuItemVm);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar menú: {ex.Message}");
            // Fallback: cargar menú por defecto
            LoadDefaultMenu();
        }
    }

    private MenuItemViewModel CreateMenuItemViewModel(MenuDto dto)
    {
        MenuItemViewModel menuItem = null!;
            menuItem = new MenuItemViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Icon = dto.Icon,
                // Map backend route/path to Shell route name using RouteMap
                Route = AutogestionSenaMaui.Helpers.RouteMap.Map(dto.Route) ?? dto.Route,
            ParentId = dto.ParentId,
            Order = dto.Order,
            IsExpanded = dto.IsExpanded,
            ToggleCommand = new Command(() => ToggleMenuItem(menuItem)),
                // Store backend route to pass through if needed
                BackendRoute = dto.Route,
                NavigateCommand = new Command(() => NavigateToRoute(menuItem.Route ?? dto.Route, dto.Route))
        };

        // Cargar submenús recursivamente
        if (dto.SubMenus != null && dto.SubMenus.Any())
        {
            foreach (var subMenu in dto.SubMenus.OrderBy(s => s.Order))
            {
                var subMenuItem = CreateMenuItemViewModel(subMenu);
                menuItem.SubMenus.Add(subMenuItem);
            }
        }

        return menuItem;
    }

    private void ToggleMenuItem(MenuItemViewModel menuItem)
    {
        menuItem.IsExpanded = !menuItem.IsExpanded;
    }

    private async void NavigateToRoute(string route, string? backendRoute = null)
    {
        if (string.IsNullOrEmpty(route))
            return;

        try
        {
            // Desmarcar todos los items
            foreach (var item in MenuItems)
            {
                item.IsSelected = false;
                foreach (var subItem in item.SubMenus)
                {
                    subItem.IsSelected = false;
                }
            }

            // Marcar el item actual
            var selectedItem = FindMenuItemByRoute(route);
            if (selectedItem != null)
            {
                selectedItem.IsSelected = true;
            }

            // Si la ruta pasada es un backend path, mapearla a Shell route
            var shellRoute = AutogestionSenaMaui.Helpers.RouteMap.Map(route) ?? route;

            // Navegar a la ruta — usar Shell si está disponible
            if (Shell.Current != null)
            {
                // If this route is the NotImplemented route (placeholder), send the original backend route as a parameter
                if (string.Equals(shellRoute, "NotImplemented", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(backendRoute))
                {
                    System.Diagnostics.Debug.WriteLine($"[SIDE-MENU] Navigating to {shellRoute} (backend={backendRoute})");
                    await Shell.Current.GoToAsync($"//{shellRoute}?backendPath={Uri.EscapeDataString(backendRoute)}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[SIDE-MENU] Navigating to {shellRoute}");
                    await Shell.Current.GoToAsync($"//{shellRoute}");
                }
            }
            else if (Application.Current?.MainPage?.Navigation != null)
            {
                // No tenemos un mapeo directo para rutas Shell, así que como fallback no haremos nada
                // o podríamos intentar mapear rutas manualmente según la convención de la app.
                System.Diagnostics.Debug.WriteLine($"[NAV] Shell.Current es null, no se pudo navegar a: {route}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al navegar: {ex.Message}");
        }
    }

    private MenuItemViewModel? FindMenuItemByRoute(string route)
    {
        foreach (var item in MenuItems)
        {
            if (item.Route == route)
                return item;

            var subItem = item.SubMenus.FirstOrDefault(s => s.Route == route);
            if (subItem != null)
                return subItem;
        }
        return null;
    }

    private void OnNavigate(string route)
    {
        NavigateToRoute(route);
    }

    private async void OnOpenProfile()
    {
        // Navegar a la página de perfil
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//profile");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("[NAV] Shell.Current es null, no se pudo navegar a profile");
        }
    }

    private void UpdateUserInitials()
    {
        if (string.IsNullOrEmpty(UserName))
        {
            UserInitials = "??";
            return;
        }

        var parts = UserName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            UserInitials = $"{parts[0][0]}{parts[1][0]}".ToUpper();
        }
        else if (parts.Length == 1)
        {
            UserInitials = parts[0].Length >= 2 
                ? parts[0].Substring(0, 2).ToUpper() 
                : parts[0].ToUpper();
        }
        else
        {
            UserInitials = "??";
        }
    }

    private void LoadDefaultMenu()
    {
        // Menú básico por defecto (por si falla la carga desde API)
        // Usando Bootstrap Icons Unicode
        MenuItems.Clear();
        
        var defaultMenu = new MenuItemViewModel
        {
            Id = 1,
            Name = "Seguridad",
            Icon = "\uf59d", // bi-shield-fill
            Route = "security",
            IsExpanded = false,
            ToggleCommand = new Command(() => { }),
            NavigateCommand = new Command(() => NavigateToRoute("security"))
        };

        var subMenu1 = new MenuItemViewModel
        {
            Id = 2,
            Name = "Asignar",
            Route = "security/assign",
            ParentId = 1,
            NavigateCommand = new Command(() => NavigateToRoute("security/assign"))
        };

        defaultMenu.SubMenus.Add(subMenu1);
        MenuItems.Add(defaultMenu);

        // Add Admin menu as fallback in default menu
        var adminMenu = new MenuItemViewModel
        {
            Id = 100,
            Name = "Administración",
            Icon = "\uf002",
            Route = "admin",
            IsExpanded = false,
            ToggleCommand = new Command(() => { }),
            NavigateCommand = new Command(() => NavigateToRoute("admin"))
        };
        MenuItems.Add(adminMenu);

        RoleName = "Usuario";
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
