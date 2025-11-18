# 🚀 Guía Rápida de Integración - Módulo de Seguridad

## ✅ Lo que ya está hecho

1. **Layout fijo completo** con diseño basado en Figma
2. **Sistema de navegación por tabs** funcional
3. **Vista de Resumen** con matriz de permisos y distribución por roles
4. **ViewModels y Converters** configurados
5. **Integración con AppShell** lista

## 📋 Pasos para usar el módulo

### 1. Agregar los iconos necesarios

Coloca los siguientes iconos en la carpeta `Resources/Images/`:

#### Iconos de tarjetas (32x32px):
- `user_icon.png` - Icono de usuarios
- `shield_icon.png` - Icono de roles
- `grid_icon.png` - Icono de módulos
- `document_icon.png` - Icono de formularios

#### Iconos de tabs (10x10px):
- `home_icon.png` - Resumen
- `users_icon.png` - Usuarios
- `shield_small_icon.png` - Roles
- `grid_small_icon.png` - Módulos

#### Iconos de permisos (16x16px):
- `check_icon.png` - ✅ Permiso activo (verde)
- `close_icon.png` - ❌ Permiso inactivo (gris)

> **Nota**: Puedes usar Font Awesome, Material Icons o crear SVG personalizados

### 2. Navegar al módulo desde cualquier página

```csharp
// Navegación simple
await Shell.Current.GoToAsync("//SecurityMainPage");

// O desde un botón
<Button Text="Ir a Seguridad" 
        Clicked="OnSecurityClicked"/>

// En el code-behind:
private async void OnSecurityClicked(object sender, EventArgs e)
{
    await Shell.Current.GoToAsync("//SecurityMainPage");
}
```

### 3. Verificar que BottomMenu existe

El layout incluye `<contentViews:BottomMenu Grid.Row="2"/>`, asegúrate de que existe en:
- `ContentViews/BottomMenu.xaml`
- `ContentViews/BottomMenu.xaml.cs`

Si no existe, comenta esa línea temporalmente:
```xml
<!-- <contentViews:BottomMenu Grid.Row="2"/> -->
```

### 4. Compilar el proyecto

```powershell
# Limpiar y compilar
dotnet clean
dotnet build

# O ejecutar directamente
dotnet build -t:Run -f net8.0-android
```

## 🎨 Personalizar el módulo

### Cambiar colores
Edita los colores en `SecurityMainPage.xaml`:

```xml
<!-- Cambiar color del header -->
<Label TextColor="#TU_COLOR" />

<!-- Cambiar color de fondo -->
<Grid BackgroundColor="#TU_COLOR">
```

### Modificar el ViewModel
Edita `ViewModels/Security/SecurityMainViewModel.cs`:

```csharp
// Cambiar valores iniciales
UsersCount = 10;  // Tu valor
RolesCount = 5;   // Tu valor

// Agregar más propiedades
public int PermissionsCount { get; set; }
```

### Agregar nuevos tabs
1. Crear nueva vista en `Views/Security/`
2. Agregar nuevo case en `LoadContent()` del ViewModel
3. Agregar nuevo Border en la sección de navegación

## 🔗 Conectar con la API

### Ejemplo de integración con ApiService:

```csharp
// En SecurityMainViewModel.cs
public class SecurityMainViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    
    public SecurityMainViewModel()
    {
        _apiService = new ApiService();
        LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        try
        {
            // Obtener contadores desde la API
            var users = await _apiService.GetAsync<List<UserDto>>("users");
            UsersCount = users.Count;
            
            var roles = await _apiService.GetAsync<List<RoleDto>>("roles");
            RolesCount = roles.Count;
            
            // Actualizar UI
            OnPropertyChanged(nameof(UsersCount));
            OnPropertyChanged(nameof(RolesCount));
        }
        catch (Exception ex)
        {
            // Manejar error
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
```

## 🧪 Probar el módulo

### Test básico
1. Ejecuta la app
2. Navega a "Seguridad"
3. Verifica que aparecen:
   - ✅ Título "Administración de Permisos"
   - ✅ 4 tarjetas de estadísticas
   - ✅ 4 tabs de navegación
   - ✅ Matriz de permisos
   - ✅ Distribución por roles

### Test de navegación
1. Toca cada tab (Resumen, Usuarios, Roles, Módulos)
2. Verifica que cambia el contenido
3. Verifica que el tab se marca como activo (fondo blanco)

## 🐛 Problemas comunes

### Error: Namespace 'AutogestionSenaMaui' no encontrado
**Causa**: Inconsistencia en los namespaces del proyecto  
**Solución**: Verificar que los archivos usen el namespace correcto según el proyecto

### Error: No aparecen las imágenes
**Causa**: Iconos no agregados o mal configurados  
**Solución**: 
1. Agregar imágenes en `Resources/Images/`
2. Verificar que estén en el .csproj:
```xml
<MauiImage Include="Resources\Images\*" />
```

### Error: Los tabs no cambian de color
**Causa**: Converters no registrados  
**Solución**: Verificar en App.xaml:
```xml
<converters:TabToColorConverter x:Key="TabToColorConverter"/>
<converters:TabToTextColorConverter x:Key="TabToTextColorConverter"/>
```

## 📱 Capturas esperadas

Una vez implementado, deberías ver:

1. **Header**: Título, subtítulo y 4 tarjetas en scroll horizontal
2. **Tabs**: 4 botones con el primero activo (blanco)
3. **Contenido**: 
   - Matriz de permisos en tabla con scroll
   - Lista de roles con colores distintivos
4. **Footer**: BottomMenu (si existe)

## 🎯 Siguiente fase de desarrollo

Una vez que el layout funcione correctamente:

1. **Implementar SecurityUsersView**
   - Lista de usuarios
   - Formulario de creación/edición
   - Búsqueda y filtros

2. **Implementar SecurityRolesView**
   - CRUD de roles
   - Asignación de permisos

3. **Implementar SecurityModulesView**
   - Gestión de módulos
   - Formularios asociados

4. **Conectar con Backend**
   - Endpoints de usuarios
   - Endpoints de roles
   - Endpoints de permisos

## 💡 Tips de desarrollo

1. **Usa datos de prueba primero** antes de conectar la API
2. **Desarrolla una vista a la vez** (completa Usuarios antes de Roles)
3. **Reutiliza componentes** (crea ContentViews para elementos repetitivos)
4. **Mantén el ViewModel limpio** (separa lógica de negocio en servicios)

## 📞 ¿Necesitas ayuda?

Si encuentras problemas:
1. Revisa este documento
2. Consulta el README_SECURITY_MODULE.md
3. Verifica los errores de compilación
4. Comprueba que todos los archivos existen

---

**Autor**: Sistema de Autogestión SENA  
**Fecha**: Noviembre 2025  
**Versión**: 1.0
