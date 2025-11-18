using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Views;

namespace AutogestionSena.MAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly UserService _userService;
        private string _currentEmail = string.Empty;
        private string _currentPassword = string.Empty;

        public LoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
            
            // Suscribirse a eventos del modal
            TwoFactorModal.CodeVerified += OnCodeVerified;
            TwoFactorModal.Cancelled += OnTwoFactorCancelled;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string? username = UsernameEntry.Text?.Trim();
            string? password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor ingresa usuario y contraseña.", "Aceptar");
                return;
            }

            try
            {
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                IsEnabled = false;

                // Guardar credenciales para usar después del 2FA
                _currentEmail = username;
                _currentPassword = password;

                var response = await _userService.ValidateLoginAsync(username, password);

                if (response != null)
                {
                    // Si hay token, significa que el login fue exitoso
                    // Ahora mostrar el modal de segundo factor
                    LoadingIndicator.IsVisible = false;
                    LoadingIndicator.IsRunning = false;
                    
                    TwoFactorModal.Show(username);
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un problema al iniciar sesión: {ex.Message}", "Aceptar");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                IsEnabled = true;
            }
        }

        private async void OnRegisterLinkTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordRecoveryPage());
        }

        private async void OnCodeVerified(object? sender, string code)
        {
            try
            {
                var request = new SecondFactorRequest
                {
                    Email = _currentEmail,
                    Code = code
                };

                // Validar el código 2FA directamente - la API devuelve los tokens
                var response = await _userService.ValidateSecondFactorAsync(request);

                if (response != null && !string.IsNullOrEmpty(response.Access))
                {
                    // Guardar tokens
                    Preferences.Set("AuthToken", response.Access);
                    if (!string.IsNullOrEmpty(response.Refresh))
                    {
                        Preferences.Set("RefreshToken", response.Refresh);
                    }
                    
                    _userService.SetAuthToken(response.Access);
                    
                    TwoFactorModal.ShowSuccess();
                    await DisplayAlert("Éxito", "Inicio de sesión correcto.", "Continuar");
                    
                    // Limpiar credenciales
                    _currentEmail = string.Empty;
                    _currentPassword = string.Empty;
                    
                    // Navegar a la página principal cuando exista
                    // await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    TwoFactorModal.ShowError("Código incorrecto. Por favor intenta de nuevo.");
                }
            }
            catch (Exception ex)
            {
                TwoFactorModal.ShowError($"Error al verificar el código: {ex.Message}");
            }
        }

        private void OnTwoFactorCancelled(object? sender, EventArgs e)
        {
            IsEnabled = true;
            _currentEmail = string.Empty;
            _currentPassword = string.Empty;
        }
    }
}
