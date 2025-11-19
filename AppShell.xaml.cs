using AutogestionSenaMaui.Views;
using AutogestionSenaMaui.Views.Security;
using AutogestionSena.MAUI.Views;

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
            // Dashboards de rol están definidos como ShellContent en AppShell.xaml
        }
    }
}
