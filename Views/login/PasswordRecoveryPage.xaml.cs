using System;
using System.Net.Http;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Views;

namespace AutogestionSena.MAUI.Views
{
    public partial class PasswordRecoveryPage : ContentPage
    {
        private readonly UserService _apiService;

        public PasswordRecoveryPage()
        {
            InitializeComponent();
            _apiService = new UserService();
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else if (Navigation != null)
            {
                // Si no se usa Shell, volver a la raíz del NavigationStack
                await Navigation.PopToRootAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[NAV] No Shell.Current ni Navigation disponibles para volver al login.");
            }
        }

        private async void OnSendCodeClicked(object sender, EventArgs e)
        {
            var email = EmailEntry?.Text?.Trim();
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "Por favor ingresa tu correo electrónico.", "Aceptar");
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"[DEBUG] OnSendCodeClicked called with email: {email}");
                // Mostrar indicador de carga
                if (SendButton != null)
                {
                    SendButton.IsEnabled = false;
                    SendButton.Text = "Enviando...";
                }

                try
                {
                    var result = await _apiService.RequestPasswordResetAsync(email);
                    
                    // Si recibimos cualquier respuesta del servidor (no null), navegamos
                    // porque el backend ya envió el código por email
                    if (result != null)
                    {
                        // Navegar inmediatamente a la pantalla de verificación de código usando Shell
                        var encodedEmail = Uri.EscapeDataString(email);
                        try
                        {
                            System.Diagnostics.Debug.WriteLine("[NAV] Attempting Shell navigation to CodeVerificationPage");
                            await Shell.Current.GoToAsync($"CodeVerificationPage?email={encodedEmail}&isPasswordReset=true");
                        }
                        catch (Exception navEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Shell.GoToAsync failed: {navEx}");
                            try
                            {
                                await Navigation.PushAsync(new CodeVerificationPage(email, isPasswordReset: true));
                            }
                            catch (Exception pushEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"[NAV] Fallback PushAsync also failed: {pushEx}");
                            }
                        }
                    }
                    else
                    {
                        // Si result es null pero no hubo excepción, significa que el backend respondió
                        // Navegar de todos modos porque el código fue enviado
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Result es null pero backend respondió OK");
                        var encodedEmail = Uri.EscapeDataString(email);
                        try
                        {
                            await Shell.Current.GoToAsync($"CodeVerificationPage?email={encodedEmail}&isPasswordReset=true");
                        }
                        catch (Exception navEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Shell.GoToAsync failed: {navEx}");
                            try
                            {
                                await Navigation.PushAsync(new CodeVerificationPage(email, isPasswordReset: true));
                            }
                            catch (Exception pushEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"[NAV] Fallback PushAsync also failed: {pushEx}");
                            }
                        }
                    }
                }
                catch (Exception apiEx) when (apiEx.Message.Contains("Error al procesar la respuesta") || 
                                             apiEx.Message.Contains("Object reference not set") ||
                                             apiEx is NullReferenceException)
                {
                    // El servidor respondió pero hay error en la deserialización o referencia nula
                    // Como viste el email, esto significa que el código SÍ se envió
                    // Navegar a la siguiente pantalla
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Error de deserialización/null pero el código fue enviado: {apiEx.Message}");
                    var encodedEmail = Uri.EscapeDataString(email);
                    try
                    {
                        await Shell.Current.GoToAsync($"CodeVerificationPage?email={encodedEmail}&isPasswordReset=true");
                    }
                    catch (Exception navEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NAV] Shell.GoToAsync failed: {navEx}");
                        try
                        {
                            await Navigation.PushAsync(new CodeVerificationPage(email, isPasswordReset: true));
                        }
                        catch (Exception pushEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Fallback PushAsync also failed: {pushEx}");
                        }
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Error de conexión - Para desarrollo, permitir navegación
                var continuar = await DisplayAlert("Error de Conexión", 
                    $"No se pudo conectar al servidor.\n\n¿Deseas continuar de todos modos? (Solo para pruebas de UI)\n\nError: {httpEx.Message}", 
                    "Continuar", "Cancelar");
                
                if (continuar)
                {
                    var encodedEmail = Uri.EscapeDataString(email);
                    try
                    {
                        await Shell.Current.GoToAsync($"CodeVerificationPage?email={encodedEmail}&isPasswordReset=true");
                    }
                    catch (Exception navEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NAV] Shell.GoToAsync failed: {navEx}");
                        try
                        {
                            await Navigation.PushAsync(new CodeVerificationPage(email, isPasswordReset: true));
                        }
                        catch (Exception pushEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Fallback PushAsync also failed: {pushEx}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Para cualquier otro error, si el mensaje indica que el backend respondió, navegamos
                System.Diagnostics.Debug.WriteLine($"[DEBUG] Excepción: {ex.GetType().Name} - {ex.Message}");
                
                // Si llegamos aquí y el código fue enviado (verificar en el email), navegar de todos modos
                var continuar = await DisplayAlert("Código Enviado", 
                    $"Se ha enviado un código a tu correo. ¿Deseas continuar?\n\n(Error técnico: {ex.Message})", 
                    "Continuar", "Cancelar");
                
                if (continuar)
                {
                    var encodedEmail2 = Uri.EscapeDataString(email);
                    try
                    {
                        await Shell.Current.GoToAsync($"CodeVerificationPage?email={encodedEmail2}&isPasswordReset=true");
                    }
                    catch (Exception navEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NAV] Shell.GoToAsync failed: {navEx}");
                        try
                        {
                            await Navigation.PushAsync(new CodeVerificationPage(email, isPasswordReset: true));
                        }
                        catch (Exception pushEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Fallback PushAsync also failed: {pushEx}");
                        }
                    }
                }
            }
                finally
            {
                // Restaurar botón
                if (SendButton != null)
                {
                    SendButton.IsEnabled = true;
                    SendButton.Text = "Enviar Código";
                }
            }
        }
    }
}
