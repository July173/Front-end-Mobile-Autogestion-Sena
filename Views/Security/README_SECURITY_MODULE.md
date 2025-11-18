# Módulo de Seguridad - Administración de Permisos

## 📋 Descripción

Este módulo implementa el sistema de administración de permisos según el diseño de Figma. Proporciona una interfaz completa para gestionar usuarios, roles, módulos y permisos desde una sola pantalla.

## 🏗️ Estructura del Proyecto

```
Views/Security/
├── SecurityMainPage.xaml              # Página principal con layout fijo
├── SecurityMainPage.xaml.cs
├── SecuritySummaryView.xaml           # Vista de resumen (Tab activo por defecto)
├── SecuritySummaryView.xaml.cs
├── SecurityUsersView.xaml             # Vista de gestión de usuarios
├── SecurityUsersView.xaml.cs
├── SecurityRolesView.xaml             # Vista de gestión de roles
├── SecurityRolesView.xaml.cs
├── SecurityModulesView.xaml           # Vista de gestión de módulos
└── SecurityModulesView.xaml.cs

ViewModels/Security/
└── SecurityMainViewModel.cs           # ViewModel principal con lógica de navegación

Converters/
└── TabConverters.cs                   # Converters para el sistema de tabs
```

## 🎨 Componentes del Layout Fijo

### 1. **Header Section** (Grid.Row="0")
- **Título Principal**: "Administración de Permisos"
- **Subtítulo**: Descripción del módulo
- **Tarjetas de Estadísticas**: 4 tarjetas horizontales con scroll
  - Usuarios (contador dinámico)
  - Roles (contador dinámico)
  - Módulos (contador dinámico)
  - Formularios (contador dinámico)
  
### 2. **Sistema de Navegación por Tabs**
- 4 tabs con indicador visual del tab activo:
  - **Resumen** (activo por defecto)
  - **Usuarios**
  - **Roles**
  - **Módulos**
- Cambio de color en tab activo (blanco) vs inactivo (gris claro)
- Iconos personalizados para cada tab

### 3. **Content Section** (Grid.Row="1")
Contenido dinámico que cambia según el tab seleccionado:

#### Tab Resumen:
- **Matriz de Permisos por Rol**: Tabla con scroll horizontal/vertical
  - Columnas: Rol, Formulario, Visualizar, Editar, Eliminar, Inhabilitar/Habilitar
  - Muestra permisos con iconos de check/close
  
- **Distribución por Roles**: Lista de roles con estadísticas
  - Administrador (3 usuarios) - Verde
  - Usuarios (22 usuarios) - Rojo
  - Aprendices (3321 usuarios) - Azul
  - Instructores (33 usuarios) - Naranja
  - Coordinadores (1 usuario) - Morado

#### Otros Tabs:
- Vistas placeholder listas para desarrollo futuro

### 4. **Bottom Menu** (Grid.Row="2")
- Menú de navegación inferior reutilizable

## 🎯 Características Implementadas

### ✅ Layout Fijo y Responsive
- Grid principal con 3 filas (Header, Content, Footer)
- ScrollView horizontal para tarjetas de estadísticas
- ScrollView bidireccional para la matriz de permisos
- Diseño adaptable a diferentes tamaños de pantalla

### ✅ Sistema de Tabs Funcional
- Navegación entre diferentes vistas
- Indicadores visuales del tab activo
- Converters personalizados para colores dinámicos
- Comandos ICommand para la interacción

### ✅ Diseño Fiel a Figma
- Colores exactos del diseño
- Espaciados y tamaños según especificaciones
- Bordes redondeados (Border con RoundRectangle)
- Tipografía y jerarquía visual

## 🔧 Configuración

### Converters Registrados en App.xaml
```xml
<converters:TabToColorConverter x:Key="TabToColorConverter"/>
<converters:TabToTextColorConverter x:Key="TabToTextColorConverter"/>
```

### Dependencias
- **.NET MAUI 8.0**
- **MVVM Pattern**
- **INotifyPropertyChanged** para binding de datos

## 📱 Recursos Necesarios

### Iconos SVG/PNG a agregar en `Resources/Images/`:
- `user_icon.png` - Icono de usuarios
- `shield_icon.png` - Icono de roles/permisos
- `grid_icon.png` - Icono de módulos
- `document_icon.png` - Icono de formularios
- `home_icon.png` - Icono de resumen (tab)
- `users_icon.png` - Icono de usuarios (tab)
- `shield_small_icon.png` - Icono de roles (tab)
- `grid_small_icon.png` - Icono de módulos (tab)
- `check_icon.png` - Icono de check (permisos activos)
- `close_icon.png` - Icono de close (permisos inactivos)

## 🚀 Próximos Pasos para Desarrollo

### 1. Implementar Vista de Usuarios
- [ ] Formulario de creación de usuarios
- [ ] Lista de usuarios con búsqueda y filtros
- [ ] Edición y eliminación de usuarios
- [ ] Asignación de roles

### 2. Implementar Vista de Roles
- [ ] CRUD completo de roles
- [ ] Asignación de permisos a roles
- [ ] Visualización de usuarios por rol

### 3. Implementar Vista de Módulos
- [ ] Gestión de módulos del sistema
- [ ] Asociación módulos-formularios
- [ ] Configuración de permisos por módulo

### 4. Conectar con API
- [ ] Integrar `ApiService` para obtener datos reales
- [ ] Implementar DTOs (RoleDto, PermissionDto, etc.)
- [ ] Manejo de estados de carga y errores
- [ ] Sincronización de datos

### 5. Agregar Recursos Visuales
- [ ] Descargar/crear iconos necesarios
- [ ] Optimizar imágenes para diferentes densidades
- [ ] Agregar animaciones de transición entre tabs

## 💡 Uso del Módulo

```csharp
// Navegación a la página de seguridad
await Navigation.PushAsync(new SecurityMainPage());

// El ViewModel se inicializa automáticamente
// Tab por defecto: "Resumen"
```

## 🎨 Paleta de Colores

```csharp
// Colores principales del módulo
#020817  // Negro/Gris oscuro (textos principales)
#64748B  // Gris medio (textos secundarios)
#E2E8F0  // Gris claro (bordes)
#F8F9FA  // Fondo general
#E1E2ED  // Fondo tabs inactivos

// Colores de roles
#E0F5CD / #16A34A  // Verde (Administrador)
#F5CDCD / #DC2626  // Rojo (Usuarios)
#CDDEF5 / #154FEF  // Azul (Aprendices)
#F5EDCD / #ED6A06  // Naranja (Instructores)
#F0CDF5 / #DA06ED  // Morado (Coordinadores)
```

## 📝 Notas Importantes

1. **Las vistas de Usuarios, Roles y Módulos están como placeholder** - Listas para implementar la lógica específica
2. **Los iconos deben agregarse manualmente** a la carpeta `Resources/Images/`
3. **El ViewModel usa datos de prueba** - Necesita conectarse a la API real
4. **El BottomMenu debe existir previamente** en `ContentViews/BottomMenu.xaml`

## 🐛 Solución de Problemas Comunes

### Error: No se encuentran los converters
**Solución**: Verificar que el namespace esté correctamente registrado en App.xaml:
```xml
xmlns:converters="clr-namespace:AutogestionSenaMaui.Converters"
```

### Error: No se cargan las vistas
**Solución**: Asegurarse de que todas las clases partial estén correctamente definidas en los archivos .xaml.cs

### Error: Imágenes no aparecen
**Solución**: Agregar las imágenes en `Resources/Images/` y asegurarse de que estén configuradas como `MauiImage` en el .csproj

## 📞 Soporte

Para dudas o problemas con este módulo, revisar:
- Documentación de .NET MAUI
- Diseño original en Figma
- README principal del proyecto
