using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.ContentViews
{
    /// <summary>
    /// TopBar - Barra superior de navegación
    /// Frame 425: Muestra breadcrumb navigation y notificaciones
    /// Similar al header en MainLayout.tsx de React
    /// </summary>
    public partial class TopBar : ContentView
    {
        public TopBar()
        {
            InitializeComponent();
            
            // Se puede compartir el mismo ViewModel que MainLayoutPage
            // o crear uno específico para TopBar si se necesita lógica independiente
        }
    }
}
