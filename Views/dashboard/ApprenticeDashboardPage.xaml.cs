using Microsoft.Maui.Controls;
using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.Storage;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSenaMaui.ContentViews;
using AutogestionSenaMaui.Helpers;

namespace AutogestionSenaMaui.Views;

public partial class ApprenticeDashboardPage : ContentPage
{
    public ApprenticeDashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Set BindingContext and load dashboard data
        var vm = new ApprenticeDashboardViewModel();
        BindingContext = vm;

        // Cambiar el breadcrumb en el MainLayout TopBar
        MainLayoutHelper.UpdateCurrentBreadcrumb("Dashboard", "Aprendiz");

        try
        {
            // Get person id saved at login
            var personId = Preferences.Get("UserPerson", 0);
            if (personId > 0)
            {
                // Resolve apprenticeId via ApprenticeService
                // TODO: Implementar ApprenticeService completo
                // var svc = new AutogestionSena.MAUI.Api.Services.ApprenticeService();
                // var apprentice = await svc.GetApprenticeByPersonAsync(personId);

                // Temporal: usar personId directamente
                if (true) // apprentice != null
                {
                    await vm.LoadAsync(personId); // int.Parse(apprentice.Id ?? "0");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ApprenticeDashboardPage] Error OnAppearing load dashboard: {ex}");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}
