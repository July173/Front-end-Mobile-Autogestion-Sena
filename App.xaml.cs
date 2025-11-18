using AutogestionSena.MAUI.Views;

namespace AutogestionSena.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Use Shell as root MainPage for consistent navigation across the app
            MainPage = new AppShell();
            
            // Capturar excepciones no controladas para ayudar en debugging en dispositivos
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[UNHANDLED] {e.ExceptionObject}");
            // Intentar mostrar una alerta amigable si hay un MainPage
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (Current?.MainPage != null)
                    {
                        await Current.MainPage.DisplayAlert("Error inesperado", "Se ha producido un error inesperado. Por favor, vuelve a intentarlo.", "Aceptar");
                    }
                });
            }
            catch { }
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[UNOBSERVED TASK] {e.Exception}");
            e.SetObserved();
        }
    }
}
