using AutogestionSena.MAUI;
using AutogestionSenaMaui.Helpers;

namespace AutogestionSena.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("bootstrap-icons.ttf", "BootstrapIcons");
                });

            // Register default route mappings between backend paths and Shell routes
            RouteMap.Register("/home", "MainDashboard");
            RouteMap.Register("/request-registration", "MainDashboard");
            RouteMap.Register("/security", "SecurityMainPage");
            RouteMap.Register("/security/users", "SecurityMainPage");
            RouteMap.Register("/admin", "AdminDashboard");

            // Load route mappings from Resources/Raw/routeMap.json if present
            try
            {
                var assembly = typeof(MauiProgram).Assembly;
                using var stream = assembly.GetManifestResourceStream("AutogestionSenaMaui.Resources.Raw.routeMap.json");
                if (stream != null)
                {
                    using var reader = new System.IO.StreamReader(stream);
                    var json = reader.ReadToEnd();
                    RouteMap.LoadFromJson(json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MauiProgram] Could not load routeMap.json: {ex}");
            }

            return builder.Build();
        }
    }
}
