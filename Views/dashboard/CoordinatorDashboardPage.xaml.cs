using Microsoft.Maui.Controls;

using AutogestionSenaMaui.ContentViews;
using AutogestionSenaMaui.Helpers;

namespace AutogestionSenaMaui.Views;

public partial class CoordinatorDashboardPage : ContentPage
{
    public CoordinatorDashboardPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainLayoutHelper.UpdateCurrentBreadcrumb("Dashboard", "Coordinador");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}
