using Microsoft.Maui.Controls;

using AutogestionSenaMaui.ContentViews;
using AutogestionSenaMaui.Helpers;
namespace AutogestionSenaMaui.Views;

public partial class HistoryRequestsInstructorPage : ContentPage
{
    public HistoryRequestsInstructorPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainLayoutHelper.UpdateCurrentBreadcrumb("Historial de Solicitudes", "Instructor");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}
