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
        System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Constructor ejecutado");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

   System.Diagnostics.Debug.WriteLine($"");
        System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] ===== ONAPPEARING INICIADO =====");
        System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

   // Set BindingContext and load dashboard data
 System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Creando ViewModel...");
        var vm = new ApprenticeDashboardViewModel();
        BindingContext = vm;
  System.Diagnostics.Debug.WriteLine($"? [ApprenticeDashboardPage] BindingContext asignado");

        // Cambiar el breadcrumb en el MainLayout TopBar
        System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Actualizando breadcrumb...");
        MainLayoutHelper.UpdateCurrentBreadcrumb("Dashboard", "Aprendiz");

        try
        {
      // Get person id saved at login
   System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Obteniendo UserPerson de Preferences...");
          var personId = Preferences.Get("UserPerson", 0);
   System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] UserPerson obtenido: {personId}");
      
            if (personId > 0)
      {
          System.Diagnostics.Debug.WriteLine($"? [ApprenticeDashboardPage] PersonId válido, iniciando carga del dashboard...");
                System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Llamando vm.LoadAsync({personId})...");
   
    // Resolve apprenticeId via ApprenticeService
          // TODO: Implementar ApprenticeService completo
            // var svc = new AutogestionSena.MAUI.Api.Services.ApprenticeService();
    // var apprentice = await svc.GetApprenticeByPersonAsync(personId);

          // Temporal: usar personId directamente
    if (true) // apprentice != null
         {
           await vm.LoadAsync(personId); // int.Parse(apprentice.Id ?? "0");
        System.Diagnostics.Debug.WriteLine($"? [ApprenticeDashboardPage] vm.LoadAsync completado");
                }
            }
            else
       {
                System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] PersonId no válido: {personId}");
 System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Verificando todas las preferencias guardadas:");
        
            // Listar todas las preferencias para debug
       try
          {
  var userEmail = Preferences.Get("UserEmail", "NULL");
      var userId = Preferences.Get("UserId", 0);
      var userRole = Preferences.Get("UserRole", 0);
          var userData = Preferences.Get("user_data", "NULL");
     
  System.Diagnostics.Debug.WriteLine($"   ?? UserEmail: {userEmail}");
      System.Diagnostics.Debug.WriteLine($"   ?? UserId: {userId}");
       System.Diagnostics.Debug.WriteLine($"   ?? UserPerson: {personId}");
  System.Diagnostics.Debug.WriteLine($"   ?? UserRole: {userRole}");
      System.Diagnostics.Debug.WriteLine($"   ?? user_data: {userData}");
                }
         catch (Exception prefEx)
         {
 System.Diagnostics.Debug.WriteLine($"? [ApprenticeDashboardPage] Error leyendo preferencias: {prefEx.Message}");
}
         }
        }
        catch (Exception ex)
        {
    System.Diagnostics.Debug.WriteLine($"");
            System.Diagnostics.Debug.WriteLine($"? [ApprenticeDashboardPage] ===== ERROR EN ONAPPEARING =====");
            System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Exception Type: {ex.GetType().Name}");
            System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] Error Message: {ex.Message}");
  System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] StackTrace: {ex.StackTrace}");
       System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] ===== FIN ERROR =====");
        }
        finally
     {
       System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] ===== ONAPPEARING COMPLETADO =====");
       System.Diagnostics.Debug.WriteLine($"");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        System.Diagnostics.Debug.WriteLine($"?? [ApprenticeDashboardPage] OnDisappearing ejecutado");
    }
}
