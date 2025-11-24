using Microsoft.Maui.Controls;

using AutogestionSenaMaui.ContentViews;
using AutogestionSenaMaui.Helpers;
namespace AutogestionSenaMaui.Views;

public partial class InstructorDashboardPage : ContentPage
{
    public InstructorDashboardPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainLayoutHelper.UpdateCurrentBreadcrumb("Dashboard", "Instructor");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}
