using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api.Services;

namespace AutogestionSena.MAUI.Views
{

    public partial class RegisterPage : ContentPage
    {
        private readonly UserService _apiService;
        public List<DocumentTypeDto> DocumentTypes { get; set; } = new();

        public RegisterPage()
        {
            InitializeComponent();
            _apiService = new UserService(new HttpClient());
            BindingContext = this;
            LoadDocumentTypes();
        }

        private async void LoadDocumentTypes()
        {
            try
            {
                DocumentTypes = await _apiService.GetDocumentTypesAsync();
                TipoDocumentoPicker.ItemsSource = DocumentTypes;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar los tipos de documento: {ex.Message}", "Aceptar");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var nombres = NombresEntry.Text?.Trim();
            var apellidos = ApellidosEntry.Text?.Trim();

            // Validación básica (ajusta según tus necesidades)
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nombres))
            {
                await DisplayAlert("Error", "Por favor completa todos los campos obligatorios.", "Aceptar");
                return;
            }

            // Construir el modelo de registro
            var nuevoUsuario = new User
            {
                Email = email,
                Person = new Person
                {
                    FirstName = nombres,
                    SecondName = apellidos,
                    // Asignar otros valores necesarios
                },
                Role = new Role
                {
                    TypeRole = "Usuario",
                    // Asignar otros valores necesarios
                }
            };

            // Llamar a la API de registro
            var resultado = await _apiService.RegisterUserAsync(nuevoUsuario);

            if (resultado != null && resultado.Success) {
                await DisplayAlert("Éxito", "Registro exitoso.", "Continuar");
                await Navigation.PopAsync();
            } else {
                await DisplayAlert("Error", "No se pudo registrar el usuario.", "Aceptar");
            }
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
