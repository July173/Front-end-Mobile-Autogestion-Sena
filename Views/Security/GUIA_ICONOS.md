# 🎨 Guía de Iconos para el Módulo de Seguridad

## 📦 Iconos Necesarios

### Lista Completa de Iconos

| Nombre del Archivo | Tamaño | Uso | Descripción |
|-------------------|--------|-----|-------------|
| `user_icon.png` | 32x32 | Tarjeta Usuarios | Icono de usuario/persona |
| `shield_icon.png` | 32x32 | Tarjeta Roles | Icono de escudo/seguridad |
| `grid_icon.png` | 32x32 | Tarjeta Módulos | Icono de cuadrícula/módulos |
| `document_icon.png` | 32x32 | Tarjeta Formularios | Icono de documento/formulario |
| `home_icon.png` | 10x10 | Tab Resumen | Icono de inicio/home |
| `users_icon.png` | 10x10 | Tab Usuarios | Icono de usuarios múltiples |
| `shield_small_icon.png` | 10x10 | Tab Roles | Icono de escudo pequeño |
| `grid_small_icon.png` | 10x10 | Tab Módulos | Icono de cuadrícula pequeña |
| `check_icon.png` | 16x16 | Permisos Activos | ✓ Check verde |
| `close_icon.png` | 16x16 | Permisos Inactivos | ✗ X gris |

---

## 🎯 Opción 1: Usar Font Awesome (Recomendado)

### Ventajas:
- ✅ Iconos vectoriales escalables
- ✅ Gratis y amplia variedad
- ✅ Fácil integración con MAUI

### Implementación:

#### 1. Descargar Font Awesome
Visita: https://fontawesome.com/download
Descarga la versión gratuita

#### 2. Agregar la fuente al proyecto
```xml
<!-- En MauiProgram.cs -->
.ConfigureFonts(fonts =>
{
    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesome"); // Agregar esta línea
});
```

#### 3. Reemplazar imágenes por labels con iconos
```xml
<!-- Antes (con imagen) -->
<Image Source="user_icon.png" WidthRequest="32" HeightRequest="32"/>

<!-- Después (con Font Awesome) -->
<Label Text="&#xf007;" 
       FontFamily="FontAwesome" 
       FontSize="32" 
       TextColor="#020817"/>
```

### Códigos Unicode de Font Awesome:

| Icono | Código | Descripción |
|-------|--------|-------------|
| 👤 Usuario | `&#xf007;` | user |
| 🛡️ Escudo | `&#xf3ed;` | shield-alt |
| 📦 Cuadrícula | `&#xf00a;` | th |
| 📄 Documento | `&#xf15b;` | file |
| 🏠 Home | `&#xf015;` | home |
| 👥 Usuarios | `&#xf0c0;` | users |
| ✓ Check | `&#xf00c;` | check |
| ✗ Close | `&#xf00d;` | times |

---

## 🎯 Opción 2: Usar Material Icons

### Ventajas:
- ✅ Diseño moderno de Google
- ✅ Gratis y bien documentado
- ✅ Consistente con Material Design

### Implementación:

#### 1. Descargar Material Icons
Visita: https://fonts.google.com/icons
Descarga el paquete de iconos

#### 2. Agregar la fuente
```csharp
// En MauiProgram.cs
fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
```

#### 3. Usar en XAML
```xml
<Label Text="&#xe7fd;" 
       FontFamily="MaterialIcons" 
       FontSize="32"/>
```

---

## 🎯 Opción 3: Crear SVG Personalizados

### Para crear iconos personalizados:

#### 1. Usar Figma para exportar
1. Abre el diseño en Figma
2. Selecciona el icono
3. Click derecho → Export → SVG
4. Guarda en `Resources/Images/`

#### 2. Convertir SVG a PNG (si es necesario)
- Usa herramientas como: https://cloudconvert.com/svg-to-png
- Exporta en múltiples tamaños (@1x, @2x, @3x)

---

## 🎯 Opción 4: Usar Iconos de Flaticon (Gratis)

### Pasos:
1. Visita: https://www.flaticon.com/
2. Busca los iconos necesarios:
   - "user icon"
   - "shield icon"
   - "grid icon"
   - "document icon"
   - "check icon"
   - "close icon"
3. Descarga en PNG (32x32, 16x16, 10x10)
4. Coloca en `Resources/Images/`

---

## 📁 Estructura de Carpetas Recomendada

```
Resources/
└── Images/
    ├── Icons/
    │   ├── Cards/          # Iconos de 32x32
    │   │   ├── user_icon.png
    │   │   ├── shield_icon.png
    │   │   ├── grid_icon.png
    │   │   └── document_icon.png
    │   ├── Tabs/           # Iconos de 10x10
    │   │   ├── home_icon.png
    │   │   ├── users_icon.png
    │   │   ├── shield_small_icon.png
    │   │   └── grid_small_icon.png
    │   └── Permissions/    # Iconos de 16x16
    │       ├── check_icon.png
    │       └── close_icon.png
```

---

## 🔧 Integración en .NET MAUI

### Configurar en AutogestionSenaMaui.csproj

```xml
<ItemGroup>
    <!-- Iconos de tarjetas -->
    <MauiImage Include="Resources\Images\Icons\Cards\user_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Cards\shield_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Cards\grid_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Cards\document_icon.png" />
    
    <!-- Iconos de tabs -->
    <MauiImage Include="Resources\Images\Icons\Tabs\home_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Tabs\users_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Tabs\shield_small_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Tabs\grid_small_icon.png" />
    
    <!-- Iconos de permisos -->
    <MauiImage Include="Resources\Images\Icons\Permissions\check_icon.png" />
    <MauiImage Include="Resources\Images\Icons\Permissions\close_icon.png" />
</ItemGroup>
```

O usar el comodín (más simple):

```xml
<ItemGroup>
    <MauiImage Include="Resources\Images\**\*.png" />
</ItemGroup>
```

---

## 🎨 Solución Temporal (Para Testing)

### Usar iconos Unicode en lugar de imágenes:

Reemplaza las imágenes temporalmente con emojis/caracteres Unicode:

```xml
<!-- Antes -->
<Image Source="user_icon.png" WidthRequest="32" HeightRequest="32"/>

<!-- Temporal (para testing) -->
<Label Text="👤" FontSize="32"/>  <!-- Usuario -->
<Label Text="🛡️" FontSize="32"/>  <!-- Escudo -->
<Label Text="📦" FontSize="32"/>  <!-- Módulos -->
<Label Text="📄" FontSize="32"/>  <!-- Formularios -->
<Label Text="🏠" FontSize="10"/>  <!-- Home -->
<Label Text="✓" FontSize="16" TextColor="Green"/>  <!-- Check -->
<Label Text="✗" FontSize="16" TextColor="Gray"/>   <!-- Close -->
```

---

## 🚀 Solución Rápida (5 minutos)

### Script PowerShell para descargar iconos básicos:

Crea un archivo `download_icons.ps1`:

```powershell
# Crear carpetas
New-Item -Path "Resources\Images\Icons\Cards" -ItemType Directory -Force
New-Item -Path "Resources\Images\Icons\Tabs" -ItemType Directory -Force
New-Item -Path "Resources\Images\Icons\Permissions" -ItemType Directory -Force

# Descargar iconos desde API gratuita (ejemplo con placeholder)
$icons = @(
    @{Name="user_icon.png"; Url="https://via.placeholder.com/32/020817/FFFFFF?text=U"},
    @{Name="shield_icon.png"; Url="https://via.placeholder.com/32/020817/FFFFFF?text=S"},
    @{Name="grid_icon.png"; Url="https://via.placeholder.com/32/020817/FFFFFF?text=G"},
    @{Name="document_icon.png"; Url="https://via.placeholder.com/32/020817/FFFFFF?text=D"},
    @{Name="home_icon.png"; Url="https://via.placeholder.com/10/020817/FFFFFF?text=H"},
    @{Name="users_icon.png"; Url="https://via.placeholder.com/10/020817/FFFFFF?text=U"},
    @{Name="shield_small_icon.png"; Url="https://via.placeholder.com/10/020817/FFFFFF?text=S"},
    @{Name="grid_small_icon.png"; Url="https://via.placeholder.com/10/020817/FFFFFF?text=G"},
    @{Name="check_icon.png"; Url="https://via.placeholder.com/16/16A34A/FFFFFF?text=%E2%9C%93"},
    @{Name="close_icon.png"; Url="https://via.placeholder.com/16/64748B/FFFFFF?text=X"}
)

foreach ($icon in $icons) {
    $folder = switch ($icon.Name) {
        {$_ -match "user_|shield_icon|grid_icon|document"} { "Cards" }
        {$_ -match "home_|users_|shield_small|grid_small"} { "Tabs" }
        {$_ -match "check_|close_"} { "Permissions" }
    }
    
    $path = "Resources\Images\Icons\$folder\$($icon.Name)"
    Invoke-WebRequest -Uri $icon.Url -OutFile $path
    Write-Host "Downloaded: $path" -ForegroundColor Green
}

Write-Host "`nAll icons downloaded successfully!" -ForegroundColor Cyan
```

Ejecuta:
```powershell
.\download_icons.ps1
```

---

## ✅ Checklist de Implementación

- [ ] Decidir fuente de iconos (Font Awesome / Material / Custom)
- [ ] Descargar/crear iconos necesarios
- [ ] Colocar en `Resources/Images/` o subcarpetas
- [ ] Verificar configuración en .csproj
- [ ] Limpiar y recompilar proyecto
- [ ] Probar en emulador/dispositivo
- [ ] Ajustar tamaños si es necesario
- [ ] Optimizar para diferentes densidades

---

## 🎨 Recomendación Final

**Para desarrollo rápido (ahora):**
Usa Font Awesome → Sin archivos, solo fuente, escalable, rápido

**Para producción (después):**
Usa iconos personalizados del diseño Figma → Consistencia total con el diseño

**Para testing inmediato:**
Usa emojis Unicode → Cero configuración, solo para prototipos

---

## 📞 Recursos Útiles

- Font Awesome: https://fontawesome.com/
- Material Icons: https://fonts.google.com/icons
- Flaticon: https://www.flaticon.com/
- Convertidor SVG→PNG: https://cloudconvert.com/
- Optimizador de PNG: https://tinypng.com/

---

**Siguiente paso**: Elige una opción y agrega los iconos para ver el módulo completamente funcional! 🎉
