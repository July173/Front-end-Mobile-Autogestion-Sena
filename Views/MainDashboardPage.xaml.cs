namespace AutogestionSenaMaui.Views;

public partial class MainDashboardPage : ContentPage
{
    public MainDashboardPage()
    {
        InitializeComponent();
        
        // Configurar el breadcrumb para esta página
        DashboardLayout.CurrentPage = "Inicio";
    }
}
