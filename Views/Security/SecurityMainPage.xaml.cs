using AutogestionSenaMaui.Views;

namespace AutogestionSenaMaui.Views.Security;

public partial class SecurityMainPage : ProtectedPage
{
    public SecurityMainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.Security.SecurityMainViewModel();
    }

    protected override async Task OnAuthorizedAccess(int userRole)
    {
        // Actualizar breadcrumb cuando se accede a Seguridad
        AutogestionSenaMaui.Helpers.MainLayoutHelper.UpdateCurrentBreadcrumb("Dashboard", "Seguridad");
        await base.OnAuthorizedAccess(userRole);
    }
}
