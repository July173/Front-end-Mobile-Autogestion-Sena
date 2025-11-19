using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSenaMaui.Views;
using AutogestionSenaMaui.Views.Security;
using Microsoft.Maui.Storage;

namespace AutogestionSena.MAUI.Views
{
    [QueryProperty(nameof(Email), "email")]
    [QueryProperty(nameof(IsPasswordReset), "isPasswordReset")]
    public partial class CodeVerificationPage : ContentPage
    {
        private readonly UserService _apiService;
        private string _email = string.Empty;
        private bool _isPasswordReset = false;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                UpdateInstructionText();
            }
        }

        public string IsPasswordReset
        {
            get => _isPasswordReset.ToString();
            set
            {
                _isPasswordReset = bool.TryParse(value, out var result) && result;
                UpdateInstructionText();
            }
        }

        public CodeVerificationPage() : this(string.Empty, false)
        {
        }

        public CodeVerificationPage(string email, bool isPasswordReset = false)
        {
            InitializeComponent();
            _apiService = new UserService();
            _email = email;
            _isPasswordReset = isPasswordReset;
            UpdateInstructionText();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // Ajustar tamaños de fuente y elementos según el ancho de pantalla
            try
            {
                if (width <= 360)
                {
                    TitleLabel.FontSize = 22;
                    SubtitleLabel.FontSize = 18;
                    InstructionLabel.FontSize = 13;
                    VerifyButton.FontSize = 16;
                    LogoImage.HeightRequest = 90;
                    LogoImage.WidthRequest = 90;
                }
                else if (width <= 420)
                {
                    TitleLabel.FontSize = 26;
                    SubtitleLabel.FontSize = 20;
                    InstructionLabel.FontSize = 15;
                    VerifyButton.FontSize = 18;
                    LogoImage.HeightRequest = 110;
                    LogoImage.WidthRequest = 110;
                }
                else
                {
                    TitleLabel.FontSize = 30;
                    SubtitleLabel.FontSize = 22;
                    InstructionLabel.FontSize = 16;
                    VerifyButton.FontSize = 18;
                    LogoImage.HeightRequest = 120;
                    LogoImage.WidthRequest = 120;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RESPONSIVE] Error resizing CodeVerificationPage: {ex}");
            }
        }

        private void UpdateInstructionText()
        {
            if (InstructionLabel != null)
            {
                if (_isPasswordReset)
                {
                    InstructionLabel.Text = "Ingresa el código que recibiste en tu correo electrónico para continuar con la recuperación de contraseña.";
                }
                else
                {
                    InstructionLabel.Text = "Ingresa el código de verificación de dos factores que recibiste en tu correo electrónico.";
                }
            }
        }

        private async void OnVerifyCodeClicked(object sender, EventArgs e)
        {
            var code = CodeEntry?.Text?.Trim();
            if (string.IsNullOrEmpty(code))
            {
                await DisplayAlert("Error", "Por favor ingresa el código de verificación.", "Aceptar");
                return;
            }

            try
            {
                if (_isPasswordReset)
                {
                    // Flujo de recuperación de contraseña
                    // El código será validado en el backend cuando se envíe junto con la nueva contraseña
                    // Navegar a la pantalla de cambio de contraseña con email y código (URL-encoded)
                    var encodedEmail = Uri.EscapeDataString(_email);
                    var encodedCode = Uri.EscapeDataString(code ?? string.Empty);
                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Navigating to PasswordResetPage with email={_email} code={code}");
                    try
                    {
                        await Shell.Current.GoToAsync($"PasswordResetPage?email={encodedEmail}&code={encodedCode}");
                    }
                    catch (Exception navEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"[NAV] Shell.GoToAsync failed: {navEx}");
                        try
                        {
                            await Navigation.PushAsync(new PasswordResetPage(_email, code ?? string.Empty));
                        }
                        catch (Exception pushEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Fallback PushAsync also failed: {pushEx}");
                        }
                    }
                }
                else
                {
                    // Flujo de 2FA para login
                    var result = await _apiService.ValidateSecondFactorAsync(new SecondFactorRequest
                    {
                        Email = _email,
                        Code = code
                    });

                    if (result != null && !string.IsNullOrEmpty(result.Access))
                    {
                        // Guardar tokens (usando Preferences en lugar de SecureStorage para evitar incompatibilidades)
                        try
                        {
                            Preferences.Set("AuthToken", result.Access ?? string.Empty);
                            Preferences.Set("RefreshToken", result.Refresh ?? string.Empty);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Error saving tokens to Preferences: {ex}");
                        }
                        

                        // Guardar datos del usuario (asegurar que el json tenga firstName y roleId para el menú dinámico)
                        try
                        {
                            int roleId = 0;
                            string firstName = string.Empty;
                            if (result.User != null)
                            {
                                roleId = result.User.Role;
                                firstName = result.User.Email ?? string.Empty;
                            }
                            else if (result.Role != null)
                            {
                                int.TryParse(result.Role.ToString(), out roleId);
                            }

                            var userToSave = new
                            {
                                firstName = firstName,
                                roleId = roleId
                            };
                            var userJson = System.Text.Json.JsonSerializer.Serialize(userToSave);
                            Preferences.Set("user_data", userJson);
                            // Guardar rol explícito
                            try
                            {
                                Preferences.Set("UserRole", roleId);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[NAV] No se pudo guardar UserRole en Preferences: {ex}");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Error saving user_data: {ex}");
                        }

                        // Navegar a la página correspondiente según el roleId. Default: MainDashboard
                        int navigateRoleId = 0;
                        if (result.User != null) navigateRoleId = result.User.Role;
                        else if (result.Role != null) navigateRoleId = result.Role.Value;

                        string route = "MainDashboard";
                        // Mapear roles a rutas según roleId (ajusta IDs según el backend)
                        // 1 -> Seguridad, 2 -> Aprendiz, 3 -> Instructor, 4 -> Coordinador, 5 -> Operador SofiaPlus
                        switch (navigateRoleId)
                        {
                            case 1:
                                route = "SecurityMainPage";
                                break;
                            case 2:
                                route = "ApprenticeDashboard";
                                break;
                            case 3:
                                route = "InstructorDashboard";
                                break;
                            case 4:
                                route = "CoordinatorDashboard";
                                break;
                            case 5:
                                route = "SofiaOperatorDashboard";
                                break;
                            default:
                                route = "MainDashboard";
                                break;
                        }

                        try
                        {
                            if (Shell.Current != null)
                            {
                                // Navegar usando Shell con reset de stack para ir al "inicio"
                                await Shell.Current.GoToAsync($"///{route}");
                            }
                            else if (Navigation != null)
                            {
                                // Fallback: usar Navigation.PushAsync
                                if (route == "SecurityMainPage")
                                    await Navigation.PushAsync(new SecurityMainPage());
                                else
                                    await Navigation.PushAsync(new MainDashboardPage());
                            }
                        }
                        catch (Exception navEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Error navegando a {route}: {navEx}");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Código inválido o expirado.", "Aceptar");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al verificar código: {ex.Message}", "Aceptar");
            }
        }
    }
}
