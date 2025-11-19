namespace AutogestionSenaMaui.ContentViews;

public partial class DynamicSideMenu : ContentView
{
    public DynamicSideMenu()
    {
        InitializeComponent();
        // Inicializar el ViewModel y asignarlo al BindingContext
        this.BindingContext = new AutogestionSenaMaui.ViewModels.DynamicSideMenuViewModel();
    }

    public AutogestionSenaMaui.ViewModels.DynamicSideMenuViewModel? ViewModel => this.BindingContext as AutogestionSenaMaui.ViewModels.DynamicSideMenuViewModel;
}
