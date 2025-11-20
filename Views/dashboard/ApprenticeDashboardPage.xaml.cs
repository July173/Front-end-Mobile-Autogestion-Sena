using Microsoft.Maui.Controls;
using AutogestionSenaMaui.ViewModels;
using Microsoft.Maui.Storage;
using AutogestionSena.MAUI.Api.Services;

namespace AutogestionSenaMaui.Views;

public partial class ApprenticeDashboardPage : ContentPage
{
    public ApprenticeDashboardPage()
    {
        InitializeComponent();
        DashboardLayout.CurrentPage = "Inicio - Aprendiz";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Set BindingContext and load dashboard data
        var vm = new ApprenticeDashboardViewModel();
        BindingContext = vm;

        try
        {
            // Get person id saved at login
            var personId = Preferences.Get("UserPerson", 0);
            if (personId > 0)
            {
                // Resolve apprenticeId via ApprenticeService
                var svc = new AutogestionSena.MAUI.Api.Services.ApprenticeService();
                var apprentice = await svc.GetApprenticeByPersonAsync(personId);
                if (apprentice != null)
                {
                    await vm.LoadAsync(int.Parse(apprentice.Id ?? "0"));
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ApprenticeDashboardPage] Error OnAppearing load dashboard: {ex}");
        }
    }
}
