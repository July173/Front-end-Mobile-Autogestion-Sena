using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSenaMaui.Helpers;
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
                    System.Diagnostics.Debug.WriteLine("===========================================");
                    System.Diagnostics.Debug.WriteLine($"[2FA] ✅ Código verificado exitosamente");
                    System.Diagnostics.Debug.WriteLine($"[2FA] 📦 RESPUESTA DEL API:");
                    System.Diagnostics.Debug.WriteLine($"[2FA]   response.User == null? {response.User == null}");
                    if (response.User != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[2FA]   response.User.Id = {response.User.Id}");
                        System.Diagnostics.Debug.WriteLine($"[2FA]   response.User.Email = {response.User.Email}");
                        System.Diagnostics.Debug.WriteLine($"[2FA]   response.User.Role = {response.User.Role}");
                        System.Diagnostics.Debug.WriteLine($"[2FA]   response.User.Person = {response.User.Person}");
                    }
                    System.Diagnostics.Debug.WriteLine("===========================================");
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
                System.Diagnostics.Debug.WriteLine("===========================================");
                System.Diagnostics.Debug.WriteLine("[LOGIN] 💾 GUARDANDO DATOS DEL USUARIO:");
                System.Diagnostics.Debug.WriteLine($"[LOGIN]   Email: {response.User.Email}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN]   UserId: {response.User.Id}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN]   UserRole: {response.User.Role}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN]   UserPerson: {response.User.Person}");
                System.Diagnostics.Debug.WriteLine($"[LOGIN]   UserRegistered: {response.User.Registered}");
                
                Preferences.Set("UserEmail", response.User.Email ?? "");
                Preferences.Set("UserId", response.User.Id);
                Preferences.Set("UserRole", response.User.Role);
                Preferences.Set("UserPerson", response.User.Person);
                Preferences.Set("UserRegistered", response.User.Registered);
                
                // Verificar que se guardó correctamente
                var savedUserId = Preferences.Get("UserId", 0);
                System.Diagnostics.Debug.WriteLine($"[LOGIN] ✅ Verificación: UserId guardado = {savedUserId}");
                System.Diagnostics.Debug.WriteLine("===========================================");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[LOGIN] ⚠️ WARNING: response.User es NULL!");
            }

            System.Diagnostics.Debug.WriteLine($"[LOGIN] Access Token: {response.Access?.Substring(0, 20)}...");
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Tokens guardados - Navegando a HomePage");

            // Configurar el token en el servicio
            _apiService.SetAuthToken(response.Access ?? "");

            // Guardar en SecureStorage tambien para consistencia con la lectura del menú
            try
            {
                var userToSave = new { firstName = response.User?.Email ?? string.Empty, roleId = response.User?.Role ?? 0 };
                var userJson = System.Text.Json.JsonSerializer.Serialize(userToSave);
                Preferences.Set("user_data", userJson);
                await SecureStorage.SetAsync("user_data", userJson);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGIN] Error guardando user_data en SecureStorage: {ex}");
            }

            await Application.Current?.MainPage?.DisplayAlert(
                "Éxito",
                "Inicio de sesión exitoso",
                "Continuar"
            )!;

            // Determinar ruta según rol y navegar
            int navigateRoleId = 0;
            if (response.User != null) navigateRoleId = response.User.Role;
            else if (response.Role != null) navigateRoleId = response.Role.Value;

            try
            {
                Preferences.Set("UserRole", navigateRoleId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGIN] No se pudo guardar UserRole en Preferences: {ex}");
            }

            string route = "HomePage";
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
                    route = "HomePage";
                    break;
            }

            System.Diagnostics.Debug.WriteLine($"[LOGIN] Navegando a {route} por role {navigateRoleId}");

            // Notificar a subscriptores que el usuario ha iniciado sesión
            try
            {
                AuthEvents.NotifyUserLoggedIn(navigateRoleId, response.User?.Email ?? string.Empty, response.Access ?? string.Empty);
            }
            catch (Exception exEvent)
            {
                System.Diagnostics.Debug.WriteLine($"[LOGIN] AuthEvents.NotifyUserLoggedIn failed: {exEvent}");
            }
            try
            {
                await Shell.Current?.GoToAsync($"///{route}")!;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NAV] Error navegando a {route}: {ex}");
            }
        }
    }
}