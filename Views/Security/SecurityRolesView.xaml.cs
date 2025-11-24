using AutogestionSenaMaui.ViewModels.Security;

namespace AutogestionSenaMaui.Views.Security;

public partial class SecurityRolesView : ContentView
{
    public SecurityRolesView()
    {
        InitializeComponent();
        BindingContext = new SecurityRolesViewModel();
    }
}
