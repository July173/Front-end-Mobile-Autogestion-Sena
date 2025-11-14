using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AutogestionSena.MAUI.Api.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            
            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Realiza una petición GET y deserializa la respuesta al tipo T
        /// </summary>
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (TaskCanceledException)
            {
                throw new Exception("La solicitud excedió el tiempo de espera. Verifica tu conexión a internet.");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"No se pudo conectar al servidor. Verifica que la URL sea correcta y el servidor esté disponible.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza una petición POST con el payload y devuelve la respuesta deserializada
        /// </summary>
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[API] POST Request to: {endpoint}");
                
                // Serializar manualmente con opciones específicas
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = false
                };
                
                var jsonContent = System.Text.Json.JsonSerializer.Serialize(payload, jsonOptions);
                System.Diagnostics.Debug.WriteLine($"[API] Payload JSON: {jsonContent}");
                
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(endpoint, content);
                
                System.Diagnostics.Debug.WriteLine($"[API] Response Status: {(int)response.StatusCode} - {response.StatusCode}");
                
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[API] Response Content: {responseContent}");
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"Error {(int)response.StatusCode} ({response.StatusCode}): {responseContent}"
                    );
                }

                // Intentar deserializar con manejo de errores mejorado
                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseContent, jsonOptions);
                }
                catch (System.Text.Json.JsonException jsonEx)
                {
                    System.Diagnostics.Debug.WriteLine($"[API] JSON Deserialization Error: {jsonEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"[API] Response was: {responseContent}");
                    throw new Exception($"Error al procesar la respuesta del servidor. Respuesta: {responseContent}", jsonEx);
                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] HttpRequestException: {ex.Message}");
                throw new Exception($"Error en la petición POST a {endpoint}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Exception: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Realiza una petición POST sin esperar respuesta deserializada
        /// </summary>
        public async Task<HttpResponseMessage> PostAsync<TRequest>(string endpoint, TRequest payload)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, payload);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en la petición POST a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza una petición PUT
        /// </summary>
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest payload)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, payload);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en la petición PUT a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Realiza una petición DELETE
        /// </summary>
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error en la petición DELETE a {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Configura el token de autenticación en los headers
        /// </summary>
        public void SetAuthToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        /// <summary>
        /// Limpia el token de autenticación
        /// </summary>
        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
