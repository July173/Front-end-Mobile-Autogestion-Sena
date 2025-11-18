namespace AutogestionSenaMaui.Views.Security;

public partial class SecurityMainPage : ContentPage
{
    public SecurityMainPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.Security.SecurityMainViewModel();
    }
}
