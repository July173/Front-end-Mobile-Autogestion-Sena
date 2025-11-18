# 🔐 Sistema de Menú Dinámico Basado en Roles

## 📋 Descripción

Sistema de menú lateral dinámico que se adapta automáticamente según los permisos del rol del usuario, consumiendo el endpoint `/security/rol-form-permissions/{id}/get-menu/`.

## 🏗️ Arquitectura

### Componentes Creados

```
Api/
├── Dtos/
│   └── MenuDto.cs                          # DTOs para menú y respuesta
ContentViews/
├── DynamicSideMenu.xaml                    # Vista del menú lateral
└── DynamicSideMenu.xaml.cs
ViewModels/
└── DynamicSideMenuViewModel.cs             # Lógica del menú dinámico
Converters/
└── BoolToSubmenuBackgroundConverter.cs     # Converter para submenús
Views/
├── MainDashboardPage.xaml                  # Página con menú integrado
└── MainDashboardPage.xaml.cs
```

## 🎨 Diseño del Menú

### Estructura Visual

```
┌────────────────────────────┐
│  🔰 Autogestión CIES       │  <- Header con logo
├────────────────────────────┤
│                            │
│  🏠 Inicio                 │  <- Siempre visible
│                            │
│  🔐 Seguridad        ▼     │  <- Menú principal
│    • Asignar              │  <- Submenús
│    • Reasignar            │     (seleccionable)
│    • Seguimiento          │
│    • Historial            │
│    • Evaluar visita       │
│                            │
│  [Más menús dinámicos]     │
│                            │
├────────────────────────────┤
│  👤 Brandon                │  <- Footer con perfil
│  🏷️  Administrador         │
└────────────────────────────┘
```

### Colores y Estilos

- **Fondo del menú**: `#388E3C` (Verde SENA)
- **Texto principal**: Blanco
- **Texto secundario**: `#AFDEBF` (Verde claro)
- **Submenú seleccionado**: `rgba(46, 125, 50, 0.8)` (Verde oscuro)
- **Badge de rol**: `#0F172A` con texto `#61F659`

## 📡 Integración con API

### Endpoint Backend

```
GET /security/rol-form-permissions/{id}/get-menu/
```

**Parámetros:**
- `id` (int): ID del rol del usuario

### Response Esperado

```json
{
  "roleId": 1,
  "roleName": "Administrador",
  "menuItems": [
    {
      "id": 1,
      "name": "Seguridad",
      "icon": "fingerprint_icon.png",
      "route": "security",
      "parentId": null,
      "order": 1,
      "isExpanded": false,
      "subMenus": [
        {
          "id": 2,
          "name": "Asignar seguimiento",
          "icon": null,
          "route": "security/assign",
          "parentId": 1,
          "order": 1,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 3,
          "name": "Reasignar",
          "icon": null,
          "route": "security/reassign",
          "parentId": 1,
          "order": 2,
          "isExpanded": false,
          "subMenus": []
        }
      ]
    }
  ]
}
```

## 🔧 Configuración

### 1. Registrar Converter en App.xaml

Ya está registrado:
```xml
<converters:BoolToSubmenuBackgroundConverter x:Key="BoolToSubmenuBackgroundConverter"/>
```

### 2. Configurar Endpoint en ApiService

Asegúrate de que la base URL esté configurada:

```csharp
// En MauiProgram.cs o donde configures el HttpClient
builder.Services.AddSingleton<ApiService>(sp =>
{
    var apiService = new ApiService();
    apiService.SetBaseUrl("https://tu-api.com/api/");
    return apiService;
});
```

### 3. Guardar Datos del Usuario al Login

```csharp
// En LoginViewModel o LoginPage después del login exitoso
var user = loginResponse.User;

// Guardar token
await SecureStorage.SetAsync("auth_token", loginResponse.Token);

// Guardar datos del usuario (necesarios para el menú)
var userJson = JsonSerializer.Serialize(user);
await SecureStorage.SetAsync("user_data", userJson);

// Navegar al dashboard
await Shell.Current.GoToAsync("//MainDashboard");
```

## 💡 Uso del Componente

### Opción 1: Uso Directo en una Página

```xml
<ContentPage xmlns:contentViews="clr-namespace:AutogestionSenaMaui.ContentViews">
    <Grid ColumnDefinitions="Auto,*">
        
        <!-- Menú Lateral -->
        <contentViews:DynamicSideMenu Grid.Column="0"/>
        
        <!-- Contenido -->
        <ScrollView Grid.Column="1">
            <!-- Tu contenido aquí -->
        </ScrollView>
        
    </Grid>
</ContentPage>
```

### Opción 2: Como Shell FlyoutHeader

```xml
<!-- En AppShell.xaml -->
<Shell xmlns:contentViews="clr-namespace:AutogestionSenaMaui.ContentViews">
    <Shell.FlyoutHeader>
        <contentViews:DynamicSideMenu/>
    </Shell.FlyoutHeader>
</Shell>
```

## 🎯 Funcionalidades

### ✅ Carga Dinámica del Menú
- Obtiene el menú desde la API según el rol
- Carga automática al inicializar
- Fallback a menú por defecto si falla la API

### ✅ Navegación Jerárquica
- Menús principales expandibles/colapsables
- Submenús con indicador visual
- Navegación automática por rutas

### ✅ Indicadores Visuales
- Submenú seleccionado con fondo destacado
- Bullet points (`•`) en submenús
- Iconos personalizados por menú

### ✅ Perfil de Usuario
- Muestra avatar con iniciales
- Nombre del usuario
- Badge con el rol
- Menú desplegable al hacer clic

## 🔄 Flujo de Funcionamiento

```
1. Usuario hace login
   ↓
2. Se guarda token y datos en SecureStorage
   ↓
3. DynamicSideMenuViewModel se inicializa
   ↓
4. Lee roleId desde SecureStorage
   ↓
5. Llama a: /security/rol-form-permissions/{roleId}/get-menu/
   ↓
6. Construye MenuItemViewModel desde MenuDto
   ↓
7. Renderiza menú dinámicamente
   ↓
8. Usuario navega por los items
   ↓
9. Marca item seleccionado visualmente
```

## 🎨 Iconos Bootstrap

El sistema usa **Bootstrap Icons** (fuente TTF) en lugar de archivos PNG.

### Configuración
1. Descargar `bootstrap-icons.ttf` desde: https://icons.getbootstrap.com/
2. Colocar en `Resources/Fonts/`
3. Ya está registrado en `MauiProgram.cs` como `"BootstrapIcons"`

### Iconos Principales del Menú

| Módulo | Bootstrap Icon | Unicode |
|--------|---------------|---------|
| Inicio | bi-house-fill | `\uf425` |
| Seguridad | bi-shield-fill | `\uf59d` |
| Usuarios | bi-people-fill | `\uf4cf` |
| Asignaciones | bi-person-workspace | `\uf4e8` |
| Notificaciones | bi-bell-fill | `\uf234` |
| Menú | bi-list | `\uf479` |
| Más opciones | bi-three-dots-vertical | `\uf602` |

Ver **GUIA_BOOTSTRAP_ICONS.md** para la lista completa de iconos disponibles.

## 🔐 Seguridad

### Token de Autenticación

El sistema automáticamente envía el token en todas las peticiones:

```csharp
// En InitializeAsync del ViewModel
var token = await SecureStorage.GetAsync("auth_token");
_apiService.SetAuthToken(token);
```

### Validación de Permisos

El backend debe validar que el usuario tiene permiso para acceder a cada ruta del menú.

## 🎨 Personalización

### Cambiar Colores del Menú

En `DynamicSideMenu.xaml`:

```xml
<!-- Fondo del menú -->
<Border BackgroundColor="#TU_COLOR">

<!-- Color de texto -->
<Label TextColor="#TU_COLOR"/>

<!-- Submenú seleccionado -->
<!-- Editar BoolToSubmenuBackgroundConverter.cs -->
```

### Agregar Iconos Personalizados

Coloca los iconos en `Resources/Images/` y referenciarlos por nombre:

```json
{
  "icon": "mi_icono_personalizado.png"
}
```

### Modificar el Footer

Edita la sección Grid.Row="2" en `DynamicSideMenu.xaml`.

## 🐛 Solución de Problemas

### El menú no se carga

**Causa 1**: No hay conexión con la API  
**Solución**: Verificar que el endpoint esté accesible

**Causa 2**: roleId no encontrado  
**Solución**: Asegurarse de guardar user_data en SecureStorage

**Causa 3**: Token inválido  
**Solución**: Verificar que el token se guardó correctamente

### Los submenús no se muestran

**Causa**: IsExpanded no está funcionando  
**Solución**: Verificar que el ToggleCommand esté vinculado correctamente

### Error de deserialización

**Causa**: El DTO no coincide con la respuesta del backend  
**Solución**: Comparar MenuDto con la estructura JSON real

## 🧪 Testing

### Probar con Datos Mock

```csharp
// En DynamicSideMenuViewModel, comentar la llamada a la API y usar:
private void LoadDefaultMenu()
{
    MenuItems.Clear();
    
    // Crear menú de prueba
    var menu1 = new MenuItemViewModel
    {
        Name = "Seguridad",
        Icon = "fingerprint_icon.png",
        Route = "security",
        IsExpanded = true
    };
    
    menu1.SubMenus.Add(new MenuItemViewModel
    {
        Name = "Asignar",
        Route = "security/assign"
    });
    
    MenuItems.Add(menu1);
}
```

### Probar Navegación

1. Ejecuta la app
2. Haz clic en un item del menú
3. Verifica que navega correctamente
4. Verifica que se marca como seleccionado

## 📊 Ejemplo de Backend (Controller)

```csharp
[HttpGet("security/rol-form-permissions/{roleId}/get-menu")]
public async Task<ActionResult<MenuResponseDto>> GetMenu(int roleId)
{
    var menu = await _securityService.GetMenuByRoleAsync(roleId);
    
    return Ok(new MenuResponseDto
    {
        RoleId = roleId,
        RoleName = menu.RoleName,
        MenuItems = menu.Items.Select(MapToDto).ToList()
    });
}
```

## ✅ Checklist de Implementación

- [x] MenuDto creado
- [x] DynamicSideMenu componente creado
- [x] DynamicSideMenuViewModel implementado
- [x] Converter registrado en App.xaml
- [ ] Iconos agregados en Resources/Images/
- [ ] Endpoint backend implementado
- [ ] Pruebas de navegación
- [ ] Pruebas con diferentes roles

## 🚀 Siguiente Fase

1. **Implementar backend**: Crear endpoint que devuelve menú según rol
2. **Agregar iconos**: Descargar/crear iconos necesarios
3. **Testing con roles reales**: Probar con Admin, Usuario, Instructor, etc.
4. **Animaciones**: Agregar transiciones suaves al expandir/colapsar
5. **Responsive**: Adaptar para diferentes tamaños de pantalla

---

**Autor**: Sistema de Autogestión SENA  
**Fecha**: Noviembre 2025  
**Versión**: 1.0
