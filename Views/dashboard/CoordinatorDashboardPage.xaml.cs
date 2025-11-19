using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Views;

public partial class CoordinatorDashboardPage : ContentPage
{
    public CoordinatorDashboardPage()
    {
        InitializeComponent();
        DashboardLayout.CurrentPage = "Inicio - Coordinador";
    }
}
