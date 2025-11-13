# Explicación de la Estructura del Proyecto MAUI

Este archivo explica la estructura, el propósito y las buenas prácticas implementadas en la plantilla de aplicación .NET MAUI encontrada en este repositorio.

## ¿Qué es este proyecto?

Es una plantilla base para aplicaciones multiplataforma usando .NET MAUI, permitiendo compilar y ejecutar en Android, iOS, MacCatalyst y Windows. Incluye ejemplos de autenticación, navegación, servicios, vistas y modelos de datos.

---

## Estructura de Carpetas y Archivos

- **App.xaml / App.xaml.cs**: Define recursos globales (colores, estilos) y el punto de entrada de la app. Se inicia en la pantalla de Login. 
  - *Buena práctica*: Centralizar recursos y definir la navegación inicial.

- **AppShell.xaml / AppShell.xaml.cs**: Implementa el patrón Shell para la navegación estructurada entre páginas. 
  - *Buena práctica*: Usar Shell simplifica la navegación y el manejo de rutas.

- **MainPage.xaml / MainPage.xaml.cs**: Página de ejemplo con un contador. 
  - *Buena práctica*: Separar la lógica (C#) de la interfaz (XAML).

- **ContentViews/**: Vistas reutilizables (por ejemplo, menús o componentes visuales).
  - *Buena práctica*: Promueve la reutilización y el mantenimiento del código.

- **Models/**: Clases de datos (DTOs, entidades).
  - *Buena práctica*: Separar la representación de datos de la lógica de negocio.

- **Services/**: Servicios para lógica de negocio, acceso a APIs, autenticación, etc.
  - *Buena práctica*: Inyección de dependencias y separación de responsabilidades.

- **ViewModels/**: Implementa el patrón MVVM, conectando vistas y modelos.
  - *Buena práctica*: Facilita pruebas unitarias y desacopla la UI de la lógica.

- **Views/**: Páginas principales de la app (Login, Home, Perfil, Registro, etc.).
  - *Buena práctica*: Cada vista tiene su archivo XAML y su code-behind.

- **Resources/**: Recursos compartidos como imágenes, fuentes, estilos y colores.
  - *Buena práctica*: Centralizar recursos para facilitar cambios globales.

- **Platforms/**: Código específico para cada plataforma (Android, iOS, Windows, MacCatalyst, Tizen).
  - *Buena práctica*: Mantener el código multiplataforma limpio y solo usar esta carpeta para personalizaciones necesarias.

- **Properties/**: Configuraciones de lanzamiento y otros metadatos.

- **bin/** y **obj/**: Carpetas generadas automáticamente para binarios y archivos temporales. No deben editarse manualmente.

- **.gitignore**: Evita que archivos/carpetas temporales o sensibles se suban al repositorio.
  - *Buena práctica*: Mantener el repositorio limpio y seguro.

- **plantilla.NetMaui.csproj / .sln**: Archivos de configuración del proyecto y la solución.

---

## Buenas Prácticas Aplicadas

- **Separación de responsabilidades**: Cada carpeta tiene un propósito claro (vistas, modelos, servicios, etc.).
- **Uso de MVVM**: Facilita el mantenimiento, escalabilidad y pruebas.
- **Centralización de recursos**: Colores, estilos e imágenes en un solo lugar.
- **Navegación con Shell**: Simplifica rutas y navegación entre páginas.
- **.gitignore**: Protege información sensible y evita archivos innecesarios en el control de versiones.
- **Compatibilidad multiplataforma**: Estructura preparada para compilar en varios sistemas operativos.

---

## ¿Por qué seguir esta estructura?

- Facilita el trabajo en equipo y el mantenimiento.
- Permite escalar la aplicación de forma ordenada.
- Hace más sencillo aplicar cambios globales (por ejemplo, cambiar un color o un estilo).
- Mejora la calidad del código y la experiencia de desarrollo.

---

**Resumen:**
Esta plantilla sigue las mejores prácticas recomendadas por Microsoft y la comunidad para proyectos .NET MAUI, asegurando un desarrollo profesional, ordenado y preparado para crecer.
