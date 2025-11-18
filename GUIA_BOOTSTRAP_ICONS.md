# 🎨 Guía de Bootstrap Icons en .NET MAUI

## 📥 Instalación

### 1. Descargar la fuente Bootstrap Icons

Descarga el archivo **bootstrap-icons.ttf** desde:
- https://github.com/twbs/icons/releases/latest
- O directamente: https://github.com/twbs/icons/releases/download/v1.11.3/bootstrap-icons-1.11.3.zip

### 2. Agregar la fuente al proyecto

1. Copia `bootstrap-icons.ttf` a la carpeta `Resources/Fonts/`
2. El proyecto ya está configurado para incluir todas las fuentes con:
   ```xml
   <MauiFont Include="Resources\Fonts\*" />
   ```

### 3. Registrar la fuente en MauiProgram.cs

Actualiza el método `ConfigureFonts`:

```csharp
.ConfigureFonts(fonts =>
{
    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    fonts.AddFont("bootstrap-icons.ttf", "BootstrapIcons");
})
```

## 🎯 Uso en XAML

### Forma 1: Directa con código Unicode

```xml
<Label Text="&#xf425;" 
       FontFamily="BootstrapIcons"
       FontSize="24"
       TextColor="White"/>
```

### Forma 2: Usando constantes (Recomendado)

```xml
<!-- Agregar namespace -->
<ContentPage xmlns:helpers="clr-namespace:AutogestionSena.MAUI.Helpers">

<!-- Usar con x:Static -->
<Label Text="{x:Static helpers:BootstrapIcons.House}"
       FontFamily="BootstrapIcons"
       FontSize="24"
       TextColor="White"/>
```

### Forma 3: Con DataBinding

```xml
<Label Text="{Binding IconCode}"
       FontFamily="BootstrapIcons"
       FontSize="24"/>
```

```csharp
// En el ViewModel
public string IconCode { get; set; } = BootstrapIcons.House;
```

## 📋 Iconos Disponibles para el Menú

### Navegación Principal
| Icono | Constante | Unicode | Uso |
|-------|-----------|---------|-----|
| 🏠 | `BootstrapIcons.House` | `\uf425` | Inicio |
| 🔐 | `BootstrapIcons.Shield` | `\uf59d` | Seguridad |
| 👤 | `BootstrapIcons.PersonWorkspace` | `\uf4e8` | Asignaciones |
| 📋 | `BootstrapIcons.ClipboardCheck` | `\uf2a6` | Formularios |
| 📊 | `BootstrapIcons.BarChart` | `\uf212` | Reportes |
| ⚙️ | `BootstrapIcons.Gear` | `\uf3e2` | Configuración |

### Acciones Comunes
| Icono | Constante | Unicode | Uso |
|-------|-----------|---------|-----|
| ➕ | `BootstrapIcons.Plus` | `\uf4fe` | Agregar |
| ✏️ | `BootstrapIcons.Pencil` | `\uf4c6` | Editar |
| 🗑️ | `BootstrapIcons.Trash` | `\uf61f` | Eliminar |
| 🔍 | `BootstrapIcons.Search` | `\uf565` | Buscar |
| ✅ | `BootstrapIcons.Check` | `\uf26a` | Confirmar |

### UI Elements
| Icono | Constante | Unicode | Uso |
|-------|-----------|---------|-----|
| 🔔 | `BootstrapIcons.Bell` | `\uf234` | Notificaciones |
| ☰ | `BootstrapIcons.Menu` | `\uf479` | Menú hamburguesa |
| ⋮ | `BootstrapIcons.ThreeDotsVertical` | `\uf602` | Más opciones |
| ▶ | `BootstrapIcons.ChevronRight` | `\uf282` | Expandir |
| ▼ | `BootstrapIcons.ChevronDown` | `\uf27f` | Colapsar |

## 🎨 Estilos Recomendados

### Style en App.xaml

```xml
<Application.Resources>
    <ResourceDictionary>
        
        <!-- Style base para iconos Bootstrap -->
        <Style x:Key="BootstrapIconBase" TargetType="Label">
            <Setter Property="FontFamily" Value="BootstrapIcons"/>
            <Setter Property="VerticalOptions" Value="Center"/>
            <Setter Property="HorizontalOptions" Value="Center"/>
        </Style>
        
        <!-- Icono pequeño (16px) -->
        <Style x:Key="BootstrapIconSmall" TargetType="Label" BasedOn="{StaticResource BootstrapIconBase}">
            <Setter Property="FontSize" Value="16"/>
        </Style>
        
        <!-- Icono mediano (24px) -->
        <Style x:Key="BootstrapIconMedium" TargetType="Label" BasedOn="{StaticResource BootstrapIconBase}">
            <Setter Property="FontSize" Value="24"/>
        </Style>
        
        <!-- Icono grande (32px) -->
        <Style x:Key="BootstrapIconLarge" TargetType="Label" BasedOn="{StaticResource BootstrapIconBase}">
            <Setter Property="FontSize" Value="32"/>
        </Style>
        
        <!-- Icono de menú (blanco) -->
        <Style x:Key="MenuIcon" TargetType="Label" BasedOn="{StaticResource BootstrapIconMedium}">
            <Setter Property="TextColor" Value="White"/>
        </Style>
        
    </ResourceDictionary>
</Application.Resources>
```

### Uso de los Styles

```xml
<!-- Icono con estilo -->
<Label Text="{x:Static helpers:BootstrapIcons.House}"
       Style="{StaticResource MenuIcon}"/>

<!-- Icono inline con tamaño personalizado -->
<Label Text="{x:Static helpers:BootstrapIcons.Bell}"
       FontFamily="BootstrapIcons"
       FontSize="20"
       TextColor="#FF6B6B"/>
```

## 🔧 Actualizar el Menú Dinámico

### Cambios en MenuDto.cs

```csharp
public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Cambiar de nombre de archivo a código Unicode
    public string Icon { get; set; } = string.Empty; // Ej: "\uf425"
    
    public string? Route { get; set; }
    public int? ParentId { get; set; }
    public int Order { get; set; }
    public bool IsExpanded { get; set; }
    public List<MenuDto> SubMenus { get; set; } = new();
}
```

### Actualizar DynamicSideMenu.xaml

```xml
<!-- Reemplazar Image por Label con icono -->
<Label Text="{Binding Icon}"
       FontFamily="BootstrapIcons"
       FontSize="24"
       TextColor="White"
       VerticalOptions="Center"
       IsVisible="{Binding HasIcon}"
       Margin="0,0,8,0"/>
```

### Backend debe enviar códigos Unicode

```json
{
  "menuItems": [
    {
      "id": 1,
      "name": "Inicio",
      "icon": "\uf425",  // Unicode de bi-house-fill
      "route": "home"
    },
    {
      "id": 2,
      "name": "Seguridad",
      "icon": "\uf59d",  // Unicode de bi-shield-fill
      "route": "security",
      "subMenus": [
        {
          "id": 3,
          "name": "Asignar",
          "icon": null,
          "route": "security/assign"
        }
      ]
    }
  ]
}
```

### O usar nombres de constantes (alternativa)

Backend envía nombre de constante:
```json
{
  "icon": "House"
}
```

ViewModel lo convierte:
```csharp
public string GetIconCode(string iconName)
{
    return iconName switch
    {
        "House" => BootstrapIcons.House,
        "Shield" => BootstrapIcons.Shield,
        "Person" => BootstrapIcons.Person,
        "Bell" => BootstrapIcons.Bell,
        _ => BootstrapIcons.Grid // Icono por defecto
    };
}
```

## 📱 Ejemplo Completo de Menú

```xml
<CollectionView ItemsSource="{Binding MenuItems}">
    <CollectionView.ItemTemplate>
        <DataTemplate>
            <Grid Padding="16,8" ColumnDefinitions="Auto,*,Auto">
                
                <!-- Icono Bootstrap -->
                <Label Grid.Column="0"
                       Text="{Binding Icon}"
                       FontFamily="BootstrapIcons"
                       FontSize="24"
                       TextColor="White"
                       VerticalOptions="Center"
                       IsVisible="{Binding HasIcon}"/>
                
                <!-- Nombre del item -->
                <Label Grid.Column="1"
                       Text="{Binding Name}"
                       FontSize="16"
                       TextColor="White"
                       VerticalOptions="Center"
                       Margin="12,0,0,0"/>
                
                <!-- Chevron para submenús -->
                <Label Grid.Column="2"
                       FontFamily="BootstrapIcons"
                       FontSize="20"
                       TextColor="White"
                       VerticalOptions="Center"
                       IsVisible="{Binding HasSubMenus}">
                    <Label.Triggers>
                        <DataTrigger TargetType="Label"
                                   Binding="{Binding IsExpanded}"
                                   Value="True">
                            <Setter Property="Text" Value="{x:Static helpers:BootstrapIcons.ChevronDown}"/>
                        </DataTrigger>
                        <DataTrigger TargetType="Label"
                                   Binding="{Binding IsExpanded}"
                                   Value="False">
                            <Setter Property="Text" Value="{x:Static helpers:BootstrapIcons.ChevronRight}"/>
                        </DataTrigger>
                    </Label.Triggers>
                </Label>
                
            </Grid>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

## 🔍 Búsqueda de Iconos

### Online
- **Sitio oficial**: https://icons.getbootstrap.com/
- Busca el icono que necesitas
- Copia el código Unicode (ejemplo: `f425`)
- Úsalo como: `\uf425`

### En el código
```csharp
// Ver BootstrapIcons.cs para la lista completa
// O buscar en: https://github.com/twbs/icons
```

## 🎯 Mapeo de Iconos del Menú SENA

| Concepto | Bootstrap Icon | Constante |
|----------|----------------|-----------|
| Inicio | bi-house-fill | `BootstrapIcons.House` |
| Seguridad | bi-shield-fill | `BootstrapIcons.Shield` |
| Permisos | bi-fingerprint | `BootstrapIcons.Fingerprint` |
| Usuarios | bi-people-fill | `BootstrapIcons.People` |
| Roles | bi-person-badge-fill | `BootstrapIcons.PersonBadge` |
| Asignar | bi-person-workspace | `BootstrapIcons.PersonWorkspace` |
| Formularios | bi-clipboard-check-fill | `BootstrapIcons.ClipboardCheck` |
| Módulos | bi-grid-fill | `BootstrapIcons.Grid` |
| Notificaciones | bi-bell-fill | `BootstrapIcons.Bell` |
| Perfil | bi-person-circle | `BootstrapIcons.PersonCircle` |
| Cerrar sesión | bi-box-arrow-right | `BootstrapIcons.BoxArrowRight` |
| Configuración | bi-gear-fill | `BootstrapIcons.Gear` |
| Menú | bi-list | `BootstrapIcons.Menu` |
| Más opciones | bi-three-dots-vertical | `BootstrapIcons.ThreeDotsVertical` |

## ✅ Checklist de Implementación

- [ ] Descargar `bootstrap-icons.ttf`
- [ ] Copiar a `Resources/Fonts/`
- [ ] Registrar en `MauiProgram.cs`
- [ ] Agregar styles en `App.xaml`
- [ ] Actualizar `DynamicSideMenu.xaml`
- [ ] Actualizar backend para enviar códigos Unicode
- [ ] Probar iconos en el menú
- [ ] Documentar iconos usados

## 🐛 Troubleshooting

### Los iconos no se muestran
1. Verifica que la fuente esté en `Resources/Fonts/`
2. Confirma el registro en `MauiProgram.cs`
3. Asegúrate de usar `FontFamily="BootstrapIcons"`
4. Verifica que el código Unicode sea correcto (`\uf425`)

### Los iconos se ven cuadrados
- El código Unicode está mal
- La fuente no se cargó correctamente
- Limpia y reconstruye el proyecto

### Diferente en Android/iOS
- Ambas plataformas deben mostrar igual
- Si hay diferencias, verifica el `FontSize`

---

**Documentación completa**: https://icons.getbootstrap.com/  
**Versión recomendada**: Bootstrap Icons 1.11.3+
