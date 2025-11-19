using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.Views;

public partial class InstructorDashboardPage : ContentPage
{
    public InstructorDashboardPage()
    {
        InitializeComponent();
        DashboardLayout.CurrentPage = "Inicio - Instructor";
    }
}
