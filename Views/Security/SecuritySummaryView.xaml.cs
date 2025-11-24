using AutogestionSenaMaui.ViewModels.Security;

namespace AutogestionSenaMaui.Views.Security;

public partial class SecuritySummaryView : ContentView
{
    public SecuritySummaryView()
    {
        InitializeComponent();
        BindingContext = new SecuritySummaryViewModel();
    }
}
