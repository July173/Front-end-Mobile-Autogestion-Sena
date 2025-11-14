using System;
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

        private async void OnSendCodeClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "Por favor ingresa tu correo electrónico.", "Aceptar");
                return;
            }

            await _apiService.RequestPasswordResetAsync(email);

            await DisplayAlert("Éxito", "Se ha enviado un código de recuperación a tu correo.", "Aceptar");
            // Navegar a la pantalla de verificación de código
            await Navigation.PushAsync(new CodeVerificationPage(email));
        }
    }
}
