using System;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api.Services;

namespace AutogestionSena.MAUI.Views
{

    public partial class RegisterPage : ContentPage
    {
        private readonly UserService _userService;
        public List<DocumentTypeDto> DocumentTypes { get; set; } = new();

        public RegisterPage()
        {
            InitializeComponent();
            _userService = new UserService();
            BindingContext = this;
            LoadDocumentTypes();
        }

        private async void LoadDocumentTypes()
        {
            try
            {
                var documentTypes = await _userService.GetDocumentTypesAsync();
                if (documentTypes != null && documentTypes.Count > 0)
                {
                    DocumentTypes = documentTypes;
                    TipoDocumentoPicker.ItemsSource = DocumentTypes;
                }
                else
                {
                    // Si no hay conexión o no hay datos, usar datos locales
                    LoadLocalDocumentTypes();
                }
            }
            catch (Exception ex)
            {
                // Si hay error de conexión, cargar datos locales sin mostrar alerta
                LoadLocalDocumentTypes();
                
                // Mostrar mensaje discreto
                Console.WriteLine($"No se pudo conectar al servidor: {ex.Message}");
            }
        }

        private void LoadLocalDocumentTypes()
        {
            // Tipos de documento comunes en Colombia
            DocumentTypes = new List<DocumentTypeDto>
            {
                new DocumentTypeDto { Id = 1, Name = "Cédula de Ciudadanía", Abbreviation = "CC", Active = true },
                new DocumentTypeDto { Id = 2, Name = "Tarjeta de Identidad", Abbreviation = "TI", Active = true },
                new DocumentTypeDto { Id = 3, Name = "Cédula de Extranjería", Abbreviation = "CE", Active = true },
                new DocumentTypeDto { Id = 4, Name = "Pasaporte", Abbreviation = "PAS", Active = true },
                new DocumentTypeDto { Id = 5, Name = "Registro Civil", Abbreviation = "RC", Active = true }
            };
            TipoDocumentoPicker.ItemsSource = DocumentTypes;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var nombres = NombresEntry.Text?.Trim();
            var apellidos = ApellidosEntry.Text?.Trim();
            var documento = DocumentoEntry.Text?.Trim();
            var telefono = TelefonoEntry.Text?.Trim();
            var tipoDocumento = TipoDocumentoPicker.SelectedItem as DocumentTypeDto;

            // Validación básica
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Error", "El correo institucional es obligatorio.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(nombres) || string.IsNullOrEmpty(apellidos))
            {
                await DisplayAlert("Error", "Los nombres y apellidos son obligatorios.", "Aceptar");
                return;
            }

            if (tipoDocumento == null)
            {
                await DisplayAlert("Error", "Debes seleccionar un tipo de documento.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(documento))
            {
                await DisplayAlert("Error", "El número de documento es obligatorio.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(telefono))
            {
                await DisplayAlert("Error", "El teléfono es obligatorio.", "Aceptar");
                return;
            }

            try
            {
                // Construir el payload de registro según la API
                var registerPayload = new RegisterPayloadDto
                {
                    Email = email,
                    FirstName = nombres,
                    FirstLastName = apellidos,
                    TypeIdentification = tipoDocumento.Id,
                    NumberIdentification = int.Parse(documento),
                    PhoneNumber = int.Parse(telefono)
                };

                // Llamar a la API de registro
                var resultado = await _userService.RegisterApprenticeAsync(registerPayload);

                if (resultado != null && resultado.Success)
                {
                    await DisplayAlert("Éxito", "Registro exitoso. Revisa tu correo para activar tu cuenta.", "Continuar");
                    await Shell.Current.GoToAsync("///LoginPage");
                }
                else
                {
                    var errorMsg = resultado?.Detail ?? "No se pudo registrar el usuario.";
                    await DisplayAlert("Error", errorMsg, "Aceptar");
                }
            }
            catch (FormatException)
            {
                await DisplayAlert("Error", "El documento y teléfono deben ser números válidos.", "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Aceptar");
            }
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
