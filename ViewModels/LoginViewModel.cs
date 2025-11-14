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
                var response = await _apiService.ValidateLoginAsync(Username, Password);

                if (response != null && !string.IsNullOrEmpty(response.Access))
                {
                    // ✅ Guardamos el token
                    Preferences.Set("AuthToken", response.Access);

                    await Application.Current?.MainPage?.DisplayAlert(
                        "Éxito",
                        "Inicio de sesión correcto.",
                        "Continuar"
                    )!;

                    // Navegar a la página principal
                    await Shell.Current?.GoToAsync("//HomePage")!;
                }
                else
                {
                    await Application.Current?.MainPage?.DisplayAlert(
                        "Error",
                        "Usuario o contraseña incorrectos.",
                        "Aceptar"
                    )!;
                }
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert(
                    "Error de conexión",
                    $"No se pudo conectar con el servidor:\n{ex.Message}",
                    "Aceptar"
                )!;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}