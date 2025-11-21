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

            // Nota: RouteMap ya no es necesario con la arquitectura simplificada
            // Ahora usamos solo LoginPage (pública) y HomePage (protegida)
            // HomePage carga dinámicamente el dashboard apropiado según el rol del usuario

            return builder.Build();
        }
    }
}
