using AutogestionSena.MAUI.Views;
using AutogestionSenaMaui.Views.Security;

namespace AutogestionSena.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas de navegación
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(CodeVerificationPage), typeof(CodeVerificationPage));
            Routing.RegisterRoute(nameof(PasswordResetPage), typeof(PasswordResetPage));
            Routing.RegisterRoute(nameof(SecurityMainPage), typeof(SecurityMainPage));
        }
    }
}
