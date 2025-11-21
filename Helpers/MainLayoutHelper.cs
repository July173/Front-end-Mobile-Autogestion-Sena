using AutogestionSenaMaui.Views;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Helpers
{
    /// <summary>
    /// Helper para trabajar con MainLayoutPage
    /// Facilita la navegación con el layout común (TopBar + Content + Footer)
    /// Similar a cómo React usa MainLayout.tsx como wrapper
    /// </summary>
    public static class MainLayoutHelper
    {
        /// <summary>
        /// Navega a una página envuelta en MainLayout
        /// </summary>
        /// <param name="content">Vista o Página a mostrar en el área de contenido</param>
        /// <param name="moduleName">Nombre del módulo para breadcrumb</param>
        /// <param name="formName">Nombre del formulario para breadcrumb</param>
        public static async Task NavigateWithLayoutAsync(object content, string moduleName = "", string formName = "")
        {
            try
            {
                var mainLayout = new MainLayoutPage();
                
                // Si es una ContentPage, extraer su contenido
                if (content is ContentPage page)
                {
                    mainLayout.SetMainContent(page.Content);
                }
                // Si es un View directamente
                else if (content is View view)
                {
                    mainLayout.SetMainContent(view);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[MAINLAYOUT_HELPER] Invalid content type: {content?.GetType().Name}");
                    return;
                }
                
                mainLayout.UpdateBreadcrumb(moduleName, formName);
                await Shell.Current.Navigation.PushAsync(mainLayout);
                
                System.Diagnostics.Debug.WriteLine($"[MAINLAYOUT_HELPER] Navigated to MainLayout with module: {moduleName}, form: {formName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAINLAYOUT_HELPER] Error navigating with layout: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza el breadcrumb de la MainLayoutPage actual
        /// </summary>
        /// <param name="moduleName">Nombre del módulo</param>
        /// <param name="formName">Nombre del formulario</param>
        public static void UpdateCurrentBreadcrumb(string moduleName, string formName)
        {
            try
            {
                // Buscar MainLayoutPage en la pila de navegación
                var currentPage = Shell.Current.Navigation.NavigationStack.LastOrDefault();
                
                if (currentPage is MainLayoutPage mainLayout)
                {
                    mainLayout.UpdateBreadcrumb(moduleName, formName);
                    System.Diagnostics.Debug.WriteLine($"[MAINLAYOUT_HELPER] Breadcrumb updated: {moduleName} > {formName}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[MAINLAYOUT_HELPER] Current page is not MainLayoutPage");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAINLAYOUT_HELPER] Error updating breadcrumb: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene la instancia de MainLayoutViewModel actual
        /// </summary>
        /// <returns>MainLayoutViewModel o null si no está disponible</returns>
        public static ViewModels.MainLayoutViewModel? GetCurrentViewModel()
        {
            try
            {
                var currentPage = Shell.Current.Navigation.NavigationStack.LastOrDefault();
                
                if (currentPage is MainLayoutPage mainLayout && mainLayout.BindingContext is ViewModels.MainLayoutViewModel viewModel)
                {
                    return viewModel;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MAINLAYOUT_HELPER] Error getting current ViewModel: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Actualiza el contador de notificaciones en el TopBar
        /// </summary>
        /// <param name="count">Cantidad de notificaciones sin leer</param>
        public static void UpdateNotificationCount(int count)
        {
            var viewModel = GetCurrentViewModel();
            viewModel?.UpdateNotificationCount(count);
        }
    }
}
