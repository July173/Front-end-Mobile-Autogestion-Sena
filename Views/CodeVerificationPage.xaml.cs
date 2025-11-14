using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.Views
{
    public partial class CodeVerificationPage : ContentPage
    {
        private readonly UserService _apiService;
        private string _email = string.Empty;

        public CodeVerificationPage() : this(string.Empty)
        {
        }

        public CodeVerificationPage(string email)
        {
            InitializeComponent();
            _apiService = new UserService();
            _email = email;
        }

        private async void OnVerifyCodeClicked(object sender, EventArgs e)
        {
            var code = CodeEntry.Text?.Trim();
            if (string.IsNullOrEmpty(code))
            {
                await DisplayAlert("Error", "Por favor ingresa el código de recuperación.", "Aceptar");
                return;
            }

            var result = await _apiService.ValidateSecondFactorAsync(new SecondFactorRequest
            {
                Email = _email,
                Code = code
            });

            await DisplayAlert("Éxito", "Código verificado correctamente.", "Aceptar");
            // Navegar a la pantalla de actualización de contraseña
            await Navigation.PushAsync(new PasswordResetPage());
        }
    }
}
