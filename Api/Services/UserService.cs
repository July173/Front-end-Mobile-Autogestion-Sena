using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api;

namespace AutogestionSena.MAUI.Api.Services
{
    public class UserService
    {
        public async Task<List<DocumentTypeDto>> GetDocumentTypesAsync()
        {
            var url = Endpoints.DocumentType.GetAll;
            return await _httpClient.GetFromJsonAsync<List<DocumentTypeDto>>(url);
        }
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var url = Endpoints.User.GetUserId(id);
            return await _httpClient.GetFromJsonAsync<User>(url);
        }

        public async Task<RegisterResponse> RegisterUserAsync(User user)
        {
            var url = Endpoints.User.GetUser;
            var response = await _httpClient.PostAsJsonAsync(url, user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RegisterResponse>();
        }

        public async Task<ValidateLoginResponse> ValidateLoginAsync(string email, string password)
        {
            var url = Endpoints.User.ValidateLogin;
            var payload = new { email, password };
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ValidateLoginResponse>();
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var url = Endpoints.User.RequestPasswordReset;
            var payload = new { email };
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ValidateSecondFactorAsync(SecondFactorRequest request)
        {
            var url = Endpoints.User.ValidateSecondFactor;
            var response = await _httpClient.PostAsJsonAsync(url, request);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        // Agrega aquí más métodos según tus necesidades
    }
}