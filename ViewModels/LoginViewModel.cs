using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.ViewModels
{
    public class LoginViewModel : BindableObject
    {
        private readonly UserService _apiService;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _isBusy;

        public LoginViewModel()
        {
            _apiService = new UserService();
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public ICommand LoginCommand { get; }

        private async Task LoginAsync()
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(Username))
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", "Ingresa tu usuario", "Aceptar")!;
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", "Ingresa tu contraseña", "Aceptar")!;
                return;
            }

            IsBusy = true;

            try
            {
                System.Diagnostics.Debug.WriteLine($"[LOGIN] Intentando login para: {Username}");
                
                var response = await _apiService.ValidateLoginAsync(Username, Password);

                System.Diagnostics.Debug.WriteLine($"[LOGIN] Respuesta recibida");

                // Si hay token en la respuesta, es porque no requiere 2FA (no debería pasar)
                if (response != null && !string.IsNullOrEmpty(response.Access))
                {
                    SaveUserDataAndNavigate(response);
                }
                else
                {
                    // Login exitoso, mostrar modal 2FA
                    await Show2FAModal(Username);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGIN ERROR] {ex}");
                
                await Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    $"Error al iniciar sesión: {ex.Message}",
                    "Aceptar"
                )!;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Show2FAModal(string email)
        {
            var code = await Application.Current?.MainPage?.DisplayPromptAsync(
                "Autenticación de Segundo Factor",
                $"Se envió un código de 6 dígitos a: {email}\n\nIngresa el código:",
                "Verificar",
                "Cancelar",
                maxLength: 6,
                keyboard: Keyboard.Numeric
            )!;

            if (!string.IsNullOrWhiteSpace(code) && code.Length == 6)
            {
                await Verify2FACode(email, code);
            }
        }

        private async Task Verify2FACode(string email, string code)
        {
            IsBusy = true;
            try
            {
                System.Diagnostics.Debug.WriteLine($"[2FA] Verificando código para: {email}");
                
                var request = new SecondFactorRequest
                {
                    Email = email,
                    Code = code
                };

                var response = await _apiService.ValidateSecondFactorAsync(request);

                if (response != null && !string.IsNullOrEmpty(response.Access))
                {
                    System.Diagnostics.Debug.WriteLine($"[2FA] Código verificado exitosamente");
                    SaveUserDataAndNavigate(response);
                }
                else
                {
                    await Application.Current?.MainPage?.DisplayAlert(
                        "Error",
                        "Código inválido o expirado",
                        "Aceptar"
                    )!;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[2FA ERROR] {ex}");
                await Application.Current?.MainPage?.DisplayAlert(
                    "Error",
                    $"Error al verificar código: {ex.Message}",
                    "Aceptar"
                )!;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void SaveUserDataAndNavigate(ValidateLoginResponse response)
        {
            // Guardar tokens
            Preferences.Set("AuthToken", response.Access ?? "");
            Preferences.Set("RefreshToken", response.Refresh ?? "");
            
            // Guardar datos del usuario
            if (response.User != null)
            {
                Preferences.Set("UserEmail", response.User.Email ?? "");
                Preferences.Set("UserId", response.User.Id);
                Preferences.Set("UserRole", response.User.Role);
                Preferences.Set("UserPerson", response.User.Person);
                Preferences.Set("UserRegistered", response.User.Registered);
                
                System.Diagnostics.Debug.WriteLine($"[LOGIN] Usuario guardado: {response.User.Email}, ID: {response.User.Id}, Role: {response.User.Role}");
            }

            System.Diagnostics.Debug.WriteLine($"[LOGIN] Access Token: {response.Access?.Substring(0, 20)}...");
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Tokens guardados - Navegando a HomePage");

            // Configurar el token en el servicio
            _apiService.SetAuthToken(response.Access ?? "");

            await Application.Current?.MainPage?.DisplayAlert(
                "Éxito",
                "Inicio de sesión exitoso",
                "Continuar"
            )!;

            // Navegar a la página principal
            await Shell.Current?.GoToAsync("//HomePage")!;
        }
    }
}