using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api;

namespace AutogestionSena.MAUI.Api.Services
{
    public class UserService
    {
        private readonly ApiService _apiService;

        public UserService()
        {
            _apiService = new ApiService();
        }

        public UserService(ApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Obtiene todos los tipos de documento disponibles
        /// </summary>
        public async Task<List<DocumentTypeDto>?> GetDocumentTypesAsync()
        {
            return await _apiService.GetAsync<List<DocumentTypeDto>>(Endpoints.DocumentType.GetAll);
        }

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _apiService.GetAsync<User>(Endpoints.User.GetUserId(id));
        }

        /// <summary>
        /// Registra un nuevo usuario (aprendiz)
        /// </summary>
        public async Task<RegisterResponse?> RegisterUserAsync(User user)
        {
            return await _apiService.PostAsync<User, RegisterResponse>(Endpoints.Person.RegisterApprentice, user);
        }

        /// <summary>
        /// Registra un nuevo aprendiz con el payload correcto
        /// </summary>
        public async Task<RegisterResponse?> RegisterApprenticeAsync(RegisterPayloadDto payload)
        {
            return await _apiService.PostAsync<RegisterPayloadDto, RegisterResponse>(Endpoints.Person.RegisterApprentice, payload);
        }

        /// <summary>
        /// Valida el login institucional del usuario
        /// </summary>
        public async Task<ValidateLoginResponse?> ValidateLoginAsync(string email, string password)
        {
            var payload = new { email, password };
            return await _apiService.PostAsync<object, ValidateLoginResponse>(Endpoints.User.ValidateLogin, payload);
        }

        /// <summary>
        /// Solicita el restablecimiento de contraseña
        /// </summary>
        public async Task RequestPasswordResetAsync(string email)
        {
            var payload = new { email };
            await _apiService.PostAsync(Endpoints.User.RequestPasswordReset, payload);
        }

        /// <summary>
        /// Valida el código de segundo factor de autenticación y retorna los tokens
        /// </summary>
        public async Task<ValidateLoginResponse?> ValidateSecondFactorAsync(SecondFactorRequest request)
        {
            return await _apiService.PostAsync<SecondFactorRequest, ValidateLoginResponse>(Endpoints.User.ValidateSecondFactor, request);
        }

        /// <summary>
        /// Configura el token de autenticación
        /// </summary>
        public void SetAuthToken(string token)
        {
            _apiService.SetAuthToken(token);
        }

        /// <summary>
        /// Limpia el token de autenticación
        /// </summary>
        public void ClearAuthToken()
        {
            _apiService.ClearAuthToken();
        }
    }
}