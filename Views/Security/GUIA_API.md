# 🔌 Guía de Conexión con API - Módulo de Seguridad

## 📋 Endpoints Necesarios

### Backend API Endpoints que necesitas:

```
Base URL: https://tu-api.com/api/

Usuarios:
  GET    /users                    - Lista de usuarios
  GET    /users/{id}               - Usuario específico
  POST   /users                    - Crear usuario
  PUT    /users/{id}               - Actualizar usuario
  DELETE /users/{id}               - Eliminar usuario
  GET    /users/stats              - Estadísticas de usuarios

Roles:
  GET    /roles                    - Lista de roles
  GET    /roles/{id}               - Rol específico
  POST   /roles                    - Crear rol
  PUT    /roles/{id}               - Actualizar rol
  DELETE /roles/{id}               - Eliminar rol
  GET    /roles/stats              - Estadísticas de roles

Permisos:
  GET    /permissions              - Lista de permisos
  GET    /permissions/matrix       - Matriz de permisos por rol
  POST   /permissions              - Asignar permiso
  DELETE /permissions/{id}         - Revocar permiso

Módulos:
  GET    /modules                  - Lista de módulos
  GET    /modules/{id}             - Módulo específico
  POST   /modules                  - Crear módulo
  PUT    /modules/{id}             - Actualizar módulo
  DELETE /modules/{id}             - Eliminar módulo
```

---

## 🔧 Actualizar SecurityMainViewModel

### Version 1: Con datos de la API

```csharp
using AutogestionSenaMaui.Api.Services;
using AutogestionSenaMaui.Api.Dtos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AutogestionSenaMaui.ViewModels.Security;

public class SecurityMainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private string _selectedTab;
    private View _currentContent;
    private bool _isLoading;
    private string _errorMessage;

    public SecurityMainViewModel()
    {
        _apiService = new ApiService();
        _selectedTab = "Resumen";
        
        SelectTabCommand = new Command<string>(OnTabSelected);
        RefreshCommand = new Command(async () => await LoadDataAsync());
        
        // Cargar datos al inicializar
        Task.Run(async () => await LoadDataAsync());
        
        LoadContent(_selectedTab);
    }

    #region Properties

    private int _usersCount;
    public int UsersCount
    {
        get => _usersCount;
        set
        {
            _usersCount = value;
            OnPropertyChanged();
        }
    }

    private int _rolesCount;
    public int RolesCount
    {
        get => _rolesCount;
        set
        {
            _rolesCount = value;
            OnPropertyChanged();
        }
    }

    private int _modulesCount;
    public int ModulesCount
    {
        get => _modulesCount;
        set
        {
            _modulesCount = value;
            OnPropertyChanged();
        }
    }

    private int _formsCount;
    public int FormsCount
    {
        get => _formsCount;
        set
        {
            _formsCount = value;
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

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public string SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (_selectedTab != value)
            {
                _selectedTab = value;
                OnPropertyChanged();
                LoadContent(value);
            }
        }
    }

    public View CurrentContent
    {
        get => _currentContent;
        set
        {
            if (_currentContent != value)
            {
                _currentContent = value;
                OnPropertyChanged();
            }
        }
    }

    // Colecciones para los datos
    public ObservableCollection<UserDto> Users { get; set; } = new();
    public ObservableCollection<RoleDto> Roles { get; set; } = new();
    public ObservableCollection<ModuleDto> Modules { get; set; } = new();
    public ObservableCollection<PermissionDto> Permissions { get; set; } = new();

    #endregion

    #region Commands

    public ICommand SelectTabCommand { get; }
    public ICommand RefreshCommand { get; }

    #endregion

    #region Methods

    private async Task LoadDataAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            // Cargar estadísticas en paralelo
            var usersTask = _apiService.GetAsync<List<UserDto>>("users");
            var rolesTask = _apiService.GetAsync<List<RoleDto>>("roles");
            var modulesTask = _apiService.GetAsync<List<ModuleDto>>("modules");
            var formsTask = _apiService.GetAsync<List<FormDto>>("forms");

            await Task.WhenAll(usersTask, rolesTask, modulesTask, formsTask);

            // Actualizar contadores
            var users = await usersTask;
            var roles = await rolesTask;
            var modules = await modulesTask;
            var forms = await formsTask;

            UsersCount = users?.Count ?? 0;
            RolesCount = roles?.Count ?? 0;
            ModulesCount = modules?.Count ?? 0;
            FormsCount = forms?.Count ?? 0;

            // Cargar colecciones completas
            Users.Clear();
            users?.ForEach(u => Users.Add(u));

            Roles.Clear();
            roles?.ForEach(r => Roles.Add(r));

            Modules.Clear();
            modules?.ForEach(m => Modules.Add(m));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error al cargar datos: {ex.Message}";
            
            // Valores por defecto en caso de error
            UsersCount = 0;
            RolesCount = 0;
            ModulesCount = 0;
            FormsCount = 0;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void OnTabSelected(string tabName)
    {
        SelectedTab = tabName;
    }

    private void LoadContent(string tabName)
    {
        CurrentContent = tabName switch
        {
            "Resumen" => new Views.Security.SecuritySummaryView { BindingContext = this },
            "Usuarios" => new Views.Security.SecurityUsersView { BindingContext = this },
            "Roles" => new Views.Security.SecurityRolesView { BindingContext = this },
            "Módulos" => new Views.Security.SecurityModulesView { BindingContext = this },
            _ => new Views.Security.SecuritySummaryView { BindingContext = this }
        };
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
```

---

## 📊 DTOs Necesarios

### Ya existen algunos, agregar los faltantes:

```csharp
// Api/Dtos/PermissionMatrixDto.cs
namespace AutogestionSenaMaui.Api.Dtos;

public class PermissionMatrixDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public int FormId { get; set; }
    public string FormName { get; set; }
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanToggle { get; set; }
}
```

```csharp
// Api/Dtos/RoleStatsDto.cs
namespace AutogestionSenaMaui.Api.Dtos;

public class RoleStatsDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public int UserCount { get; set; }
    public string ColorCode { get; set; } // Para el color de la tarjeta
}
```

---

## 🔄 Actualizar ApiService

### Agregar métodos específicos para el módulo de seguridad:

```csharp
// Api/Services/ApiService.cs
namespace AutogestionSenaMaui.Api.Services;

public partial class ApiService
{
    // Método genérico GET ya debe existir, si no:
    public async Task<T> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            // Log del error
            Console.WriteLine($"Error en GET {endpoint}: {ex.Message}");
            throw;
        }
    }

    // Métodos específicos del módulo de seguridad
    public async Task<List<PermissionMatrixDto>> GetPermissionMatrixAsync()
    {
        return await GetAsync<List<PermissionMatrixDto>>("permissions/matrix");
    }

    public async Task<List<RoleStatsDto>> GetRoleStatsAsync()
    {
        return await GetAsync<List<RoleStatsDto>>("roles/stats");
    }

    public async Task<SecurityStatsDto> GetSecurityStatsAsync()
    {
        return await GetAsync<SecurityStatsDto>("security/stats");
    }
}
```

---

## 🔧 Actualizar SecuritySummaryView

### Conectar la matriz de permisos con datos reales:

```csharp
// ViewModels/Security/SecuritySummaryViewModel.cs
using AutogestionSenaMaui.Api.Services;
using AutogestionSenaMaui.Api.Dtos;
using System.Collections.ObjectModel;

namespace AutogestionSenaMaui.ViewModels.Security;

public class SecuritySummaryViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    
    public ObservableCollection<PermissionMatrixDto> PermissionMatrix { get; set; } = new();
    public ObservableCollection<RoleStatsDto> RoleStats { get; set; } = new();

    public SecuritySummaryViewModel()
    {
        _apiService = new ApiService();
        LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            // Cargar matriz de permisos
            var matrix = await _apiService.GetPermissionMatrixAsync();
            PermissionMatrix.Clear();
            matrix?.ForEach(p => PermissionMatrix.Add(p));

            // Cargar estadísticas de roles
            var stats = await _apiService.GetRoleStatsAsync();
            RoleStats.Clear();
            stats?.ForEach(s => RoleStats.Add(s));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // INotifyPropertyChanged implementation...
}
```

### Actualizar SecuritySummaryView.xaml para usar binding:

```xml
<!-- Reemplazar la tabla estática por una dinámica -->
<CollectionView ItemsSource="{Binding PermissionMatrix}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Grid ColumnDefinitions="150,180,80,80,80,120" Padding="8">
                <Label Grid.Column="0" Text="{Binding RoleName}" />
                <Label Grid.Column="1" Text="{Binding FormName}" />
                <Image Grid.Column="2" 
                       Source="{Binding CanView, Converter={StaticResource BoolToIconConverter}}" 
                       WidthRequest="16" HeightRequest="16" />
                <Image Grid.Column="3" 
                       Source="{Binding CanEdit, Converter={StaticResource BoolToIconConverter}}" 
                       WidthRequest="16" HeightRequest="16" />
                <Image Grid.Column="4" 
                       Source="{Binding CanDelete, Converter={StaticResource BoolToIconConverter}}" 
                       WidthRequest="16" HeightRequest="16" />
                <Image Grid.Column="5" 
                       Source="{Binding CanToggle, Converter={StaticResource BoolToIconConverter}}" 
                       WidthRequest="16" HeightRequest="16" />
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

---

## 🔄 Crear Converter para Iconos

```csharp
// Converters/BoolToIconConverter.cs
using System.Globalization;

namespace AutogestionSenaMaui.Converters;

public class BoolToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "check_icon.png" : "close_icon.png";
        }
        return "close_icon.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
```

Registrar en App.xaml:
```xml
<converters:BoolToIconConverter x:Key="BoolToIconConverter"/>
```

---

## 🔐 Manejo de Autenticación

### Agregar token a las peticiones:

```csharp
// Api/Services/ApiService.cs
public class ApiService
{
    private readonly HttpClient _httpClient;
    private string _authToken;

    public void SetAuthToken(string token)
    {
        _authToken = token;
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    // Método para obtener token del storage
    public async Task<bool> InitializeAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                SetAuthToken(token);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
```

---

## 🚀 Integración Completa

### Flujo completo desde el login hasta el módulo:

```csharp
// En LoginPage.xaml.cs o LoginViewModel
private async Task OnLoginSuccessAsync(string token, UserDto user)
{
    // Guardar token
    await SecureStorage.SetAsync("auth_token", token);
    
    // Inicializar ApiService con el token
    var apiService = new ApiService();
    apiService.SetAuthToken(token);
    
    // Navegar al módulo de seguridad
    await Shell.Current.GoToAsync("//SecurityMainPage");
}
```

---

## 📊 Ejemplo de Response del Backend

### Ejemplo: GET /security/stats

```json
{
  "usersCount": 3377,
  "rolesCount": 5,
  "modulesCount": 8,
  "formsCount": 24,
  "activeUsersCount": 3350,
  "inactiveUsersCount": 27
}
```

### Ejemplo: GET /permissions/matrix

```json
[
  {
    "roleId": 1,
    "roleName": "Administrador",
    "formId": 1,
    "formName": "Crear Usuario",
    "canView": true,
    "canEdit": true,
    "canDelete": false,
    "canToggle": false
  },
  {
    "roleId": 1,
    "roleName": "Administrador",
    "formId": 2,
    "formName": "Editar Usuario",
    "canView": false,
    "canEdit": false,
    "canDelete": false,
    "canToggle": false
  }
]
```

### Ejemplo: GET /roles/stats

```json
[
  {
    "roleId": 1,
    "roleName": "Administrador",
    "description": "Acceso completo al sistema",
    "userCount": 3,
    "colorCode": "#E0F5CD"
  },
  {
    "roleId": 2,
    "roleName": "Usuario",
    "description": "Usuarios del sistema",
    "userCount": 22,
    "colorCode": "#F5CDCD"
  }
]
```

---

## 🧪 Testing de la API

### Probar endpoints con Postman o curl:

```bash
# GET: Obtener estadísticas
curl -X GET https://tu-api.com/api/security/stats \
  -H "Authorization: Bearer TU_TOKEN"

# GET: Obtener matriz de permisos
curl -X GET https://tu-api.com/api/permissions/matrix \
  -H "Authorization: Bearer TU_TOKEN"

# POST: Crear usuario
curl -X POST https://tu-api.com/api/users \
  -H "Authorization: Bearer TU_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan@example.com",
    "roleId": 2
  }'
```

---

## ✅ Checklist de Integración

- [ ] Backend tiene endpoints implementados
- [ ] DTOs coinciden con el backend
- [ ] ApiService configurado correctamente
- [ ] Token de autenticación se envía en headers
- [ ] Manejo de errores implementado
- [ ] Loading states configurados
- [ ] ViewModels actualizados con datos reales
- [ ] Bindings en XAML actualizados
- [ ] Converters creados y registrados
- [ ] Probado en emulador/dispositivo real

---

## 🐛 Solución de Problemas

### Error 401 Unauthorized
**Causa**: Token inválido o expirado  
**Solución**: Verificar que el token se esté enviando correctamente

### Error de deserialización
**Causa**: DTOs no coinciden con la respuesta del backend  
**Solución**: Usar herramientas como json2csharp.com para generar DTOs

### Timeout en peticiones
**Causa**: Backend lento o no disponible  
**Solución**: Aumentar timeout en HttpClient

```csharp
_httpClient.Timeout = TimeSpan.FromSeconds(30);
```

---

**Siguiente paso**: Implementar los endpoints en el backend y conectar con el frontend! 🚀
