using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Views
{
    /// <summary>
    /// MainLayoutPage - Página de layout principal de la aplicación
    /// Similar a MainLayout.tsx en React, proporciona estructura común:
    /// - TopBar con navegación breadcrumb y notificaciones
    /// - Área de contenido dinámico
    /// - Footer con información del sistema
    /// </summary>
    public partial class MainLayoutPage : ContentPage
    {
        private readonly MainLayoutViewModel _viewModel;

        public MainLayoutPage()
        {
            InitializeComponent();
            _viewModel = new MainLayoutViewModel();
            BindingContext = _viewModel;
        }

        /// <summary>
        /// Establece el contenido principal de la página
        /// </summary>
        /// <param name="content">View a mostrar en el área de contenido</param>
        public void SetMainContent(View content)
        {
            MainContent.Content = content;
        }

        /// <summary>
        /// Actualiza el breadcrumb de navegación
        /// Similar a handleMenuItemClick en MainLayout.tsx
        /// </summary>
        /// <param name="moduleName">Nombre del módulo activo</param>
        /// <param name="formName">Nombre del formulario activo</param>
        public void UpdateBreadcrumb(string moduleName, string formName)
        {
            _viewModel.ActiveModule = moduleName;
            _viewModel.ActiveFormName = formName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            System.Diagnostics.Debug.WriteLine("[MAINLAYOUT] MainLayoutPage appeared");
        }
    }
}
