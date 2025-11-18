using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;

namespace AutogestionSena.MAUI.Views
{
    [QueryProperty(nameof(Email), "email")]
    [QueryProperty(nameof(Code), "code")]
    public partial class PasswordResetPage : ContentPage
    {
        private readonly UserService _apiService;
        private string _email = string.Empty;
        private string _code = string.Empty;

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string Code
        {
            get => _code;
            set => _code = value;
        }

        public PasswordResetPage() : this(string.Empty, string.Empty)
        {
        }

        public PasswordResetPage(string email) : this(email, string.Empty)
        {
        }

        public PasswordResetPage(string email, string code)
        {
            InitializeComponent();
            _apiService = new UserService();
            _email = email;
            _code = code;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            try
            {
                if (width <= 360)
                {
                    ResetTitleLabel.FontSize = 20;
                    ResetSubtitleLabel.FontSize = 18;
                    ResetLogoImage.HeightRequest = 60;
                    ResetLogoImage.WidthRequest = 60;
                    ResetButton.FontSize = 16;
                }
                else if (width <= 420)
                {
                    ResetTitleLabel.FontSize = 24;
                    ResetSubtitleLabel.FontSize = 20;
                    ResetLogoImage.HeightRequest = 80;
                    ResetLogoImage.WidthRequest = 80;
                    ResetButton.FontSize = 18;
                }
                else
                {
                    ResetTitleLabel.FontSize = 26;
                    ResetSubtitleLabel.FontSize = 22;
                    ResetLogoImage.HeightRequest = 90;
                    ResetLogoImage.WidthRequest = 90;
                    ResetButton.FontSize = 18;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RESPONSIVE] Error resizing PasswordResetPage: {ex}");
            }
        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            var newPassword = NewPasswordEntry.Text?.Trim();
            var confirmPassword = ConfirmPasswordEntry.Text?.Trim();

            // Validaciones
            if (string.IsNullOrEmpty(newPassword))
            {
                await DisplayAlert("Error", "Por favor ingresa la nueva contraseña.", "Aceptar");
                return;
            }

            if (newPassword.Length < 8)
            {
                await DisplayAlert("Error", "La contraseña debe tener al menos 8 caracteres.", "Aceptar");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden.", "Aceptar");
                return;
            }

            try
            {
                // Intentar restablecer contraseña y obtener la respuesta parseada
                var parsed = await _apiService.ResetPasswordParsedAsync(_email, newPassword);

                if (parsed != null && parsed.Success)
                {
                    await DisplayAlert("Éxito", "Tu contraseña ha sido restablecida correctamente.", "Aceptar");

                    // Volver al login
                    if (Shell.Current != null)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else if (Navigation != null)
                    {
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    string detail = "No se pudo restablecer la contraseña.";
                    if (parsed != null && !string.IsNullOrEmpty(parsed.Detail)) detail = parsed.Detail;
                    await DisplayAlert("Error", detail, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", 
                    $"Error al restablecer contraseña: {ex.Message}", 
                    "Aceptar");
            }
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else if (Navigation != null)
            {
                await Navigation.PopToRootAsync();
            }
        }
    }
}
