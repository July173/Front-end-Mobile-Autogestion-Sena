using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSenaMaui.Views;
using AutogestionSenaMaui.Views.Security;
using AutogestionSenaMaui.Helpers;
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
                    await Shell.Current.GoToAsync($"///PasswordResetPage?email={encodedEmail}&code={encodedCode}");
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
                        int roleId = 0;
                        string firstName = string.Empty;
                        try
                        {
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
                            // Guardar también en SecureStorage para que el DynamicSideMenu lo lea
                            try
                            {
                                await SecureStorage.SetAsync("user_data", userJson);
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"[NAV] SecureStorage.SetAsync user_data failed: {ex}");
                            }
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

                        // Configurar token para el servicio para llamadas subsecuentes (ej: cargar menú)
                        try
                        {
                            _apiService.SetAuthToken(result.Access ?? string.Empty);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[NAV] Error setting token in UserService: {ex}");
                        }

                        // Notificar a subscriptores (DynamicSideMenuViewModel) que el usuario ha iniciado sesión

                        // Navegar a la página correspondiente según el roleId. Default: MainDashboard
                        int navigateRoleId = 0;
                        if (result.User != null) navigateRoleId = result.User.Role;
                        else if (result.Role != null) navigateRoleId = result.Role.Value;

                        // Navegar a HomePage que cargará el dashboard apropiado según el rol
                        string route = "HomePage";
                        
                        System.Diagnostics.Debug.WriteLine($"[CODE-VERIFY] Usuario con rol {navigateRoleId} ({NavigationHelper.GetRoleName(navigateRoleId)}) será redirigido a: {route}");

                            // Notificar a subscriptores (DynamicSideMenuViewModel) que el usuario ha iniciado sesión
                            try
                            {
                                AuthEvents.NotifyUserLoggedIn(navigateRoleId, firstName, result.Access ?? string.Empty);
                            }
                            catch (Exception exEvent)
                            {
                                System.Diagnostics.Debug.WriteLine($"[NAV] AuthEvents.NotifyUserLoggedIn failed: {exEvent}");
                            }

                        try
                        {
                            // Usar NavigationHelper para navegación segura
                            await NavigationHelper.NavigateToAsync(route);
                        }
                        catch (Exception navEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"[CODE-VERIFY] Error navegando a {route}: {navEx}");
                            await DisplayAlert("Error", "No se pudo navegar al dashboard.", "Aceptar");
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
