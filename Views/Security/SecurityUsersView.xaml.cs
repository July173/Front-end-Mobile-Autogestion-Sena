using AutogestionSenaMaui.ViewModels.Security;

namespace AutogestionSenaMaui.Views.Security;

public partial class SecurityUsersView : ContentView
{
    public SecurityUsersView()
    {
        InitializeComponent();
        BindingContext = new SecurityUsersViewModel();
    }
}
