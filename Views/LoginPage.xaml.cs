using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Views;

namespace AutogestionSena.MAUI.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly UserService _apiService;

        public LoginPage()
        {
            InitializeComponent();
            _apiService = new UserService(new HttpClient());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

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

                var response = await _apiService.ValidateLoginAsync(username, password);

                if (response != null && !string.IsNullOrEmpty(response.Access))
                {
                    await DisplayAlert("Éxito", "Inicio de sesión correcto.", "Continuar");
                    Preferences.Set("AuthToken", response.Access);

                    // Navegar a la página principal cuando exista
                    // await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contraseña incorrectos o servidor no disponible.", "Aceptar");
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
    }
}
