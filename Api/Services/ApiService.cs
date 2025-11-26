using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutogestionSena.MAUI.Api;

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
                BaseAddress = new Uri(Endpoints.API_BASE_URL),
                Timeout = TimeSpan.FromSeconds(30)
            };
            
            System.Diagnostics.Debug.WriteLine($"[ApiService] BaseAddress configurada: {Endpoints.API_BASE_URL}");
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
                // 🔍 LOGS DETALLADOS DE PETICIÓN HTTP
         System.Diagnostics.Debug.WriteLine($"");
     System.Diagnostics.Debug.WriteLine($"🌐 [ApiService] ===== INICIANDO PETICIÓN HTTP GET =====");
       System.Diagnostics.Debug.WriteLine($"📅 [ApiService] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
        System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] Endpoint recibido: {endpoint}");
   System.Diagnostics.Debug.WriteLine($"🏠 [ApiService] Base Address: {_httpClient.BaseAddress}");
           
          // 🔧 NUEVO: Detectar si el endpoint ya es una URL completa
       string finalUrl;
           bool isFullUrl = Uri.IsWellFormedUriString(endpoint, UriKind.Absolute);
     
 if (isFullUrl)
            {
     // Es una URL completa, usarla directamente
                    finalUrl = endpoint;
        System.Diagnostics.Debug.WriteLine($"✅ [ApiService] Detectada URL completa, usando directamente");
    }
       else
     {
     // Es un endpoint relativo, construir URL completa
        finalUrl = $"{_httpClient.BaseAddress}{endpoint}";
    System.Diagnostics.Debug.WriteLine($"✅ [ApiService] Detectado endpoint relativo, construyendo URL completa");
             }
             
      System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] URL Final: {finalUrl}");
      System.Diagnostics.Debug.WriteLine($"⏱️ [ApiService] Timeout: {_httpClient.Timeout.TotalSeconds}s");
        System.Diagnostics.Debug.WriteLine($"📊 [ApiService] Tipo esperado: {typeof(T).Name}");
    
       // Información de headers
                if (_httpClient.DefaultRequestHeaders.Authorization != null)
    {
     System.Diagnostics.Debug.WriteLine($"🔐 [ApiService] Authorization Header: {_httpClient.DefaultRequestHeaders.Authorization.Scheme} ***");
    }
  else
     {
              System.Diagnostics.Debug.WriteLine($"🔓 [ApiService] Sin Authorization Header");
          }

       System.Diagnostics.Debug.WriteLine($"🚀 [ApiService] Enviando petición...");
     var startTime = DateTime.Now;

     // 🔧 NUEVO: Usar la URL apropiada según el tipo detectado
        HttpResponseMessage response;
       if (isFullUrl)
                {
      // Para URLs completas, crear un cliente temporal o usar GetAsync con URI
    response = await _httpClient.GetAsync(new Uri(finalUrl));
     }
                else
        {
          // Para endpoints relativos, usar el método normal
     response = await _httpClient.GetAsync(endpoint);
         }

         var endTime = DateTime.Now;
   var duration = endTime - startTime;

                // 🔍 LOGS DETALLADOS DE RESPUESTA HTTP
     System.Diagnostics.Debug.WriteLine($"");
  System.Diagnostics.Debug.WriteLine($"📥 [ApiService] ===== RESPUESTA HTTP RECIBIDA =====");
           System.Diagnostics.Debug.WriteLine($"⏱️ [ApiService] Duración: {duration.TotalMilliseconds:F0}ms");
       System.Diagnostics.Debug.WriteLine($"📊 [ApiService] Status Code: {(int)response.StatusCode} - {response.StatusCode}");
    System.Diagnostics.Debug.WriteLine($"✅ [ApiService] IsSuccessStatusCode: {response.IsSuccessStatusCode}");
        System.Diagnostics.Debug.WriteLine($"📄 [ApiService] Content-Type: {response.Content.Headers.ContentType?.MediaType ?? "NULL"}");
      System.Diagnostics.Debug.WriteLine($"📏 [ApiService] Content-Length: {response.Content.Headers.ContentLength?.ToString() ?? "NULL"}");

      // Log de headers de respuesta importantes
       if (response.Headers.Contains("Server"))
  {
         System.Diagnostics.Debug.WriteLine($"🖥️ [ApiService] Server: {string.Join(", ", response.Headers.GetValues("Server"))}");
     }

        if (!response.IsSuccessStatusCode)
       {
var errorContent = await response.Content.ReadAsStringAsync();
  System.Diagnostics.Debug.WriteLine($"❌ [ApiService] Error Content Length: {errorContent?.Length ?? 0} chars");
     System.Diagnostics.Debug.WriteLine($"❌ [ApiService] Error Content: {errorContent}");

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
             {
      throw new Exception($"Endpoint no encontrado: {finalUrl}. Verifica la URL del servidor.");
    }
    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
   {
              throw new Exception($"Error interno del servidor. Contacta al administrador.");
        }
else
   {
          throw new Exception($"Error {(int)response.StatusCode}: {errorContent}");
           }
      }

       var content = await response.Content.ReadAsStringAsync();
    System.Diagnostics.Debug.WriteLine($"✅ [ApiService] Response Content Length: {content?.Length ?? 0} chars");

     // Log del contenido solo si es pequeño (para evitar spam en logs)
      if (content != null && content.Length < 2000)
    {
         System.Diagnostics.Debug.WriteLine($"📄 [ApiService] Response Content: {content}");
    }
          else if (content != null)
          {
 System.Diagnostics.Debug.WriteLine($"📄 [ApiService] Response Content (primeros 500 chars): {content.Substring(0, Math.Min(500, content.Length))}...");
   }

         // Deserializar con opciones específicas
  System.Diagnostics.Debug.WriteLine($"🔄 [ApiService] Iniciando deserialización...");
   var options = new System.Text.Json.JsonSerializerOptions
             {
       PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
    };

           var result = System.Text.Json.JsonSerializer.Deserialize<T>(content, options);

        System.Diagnostics.Debug.WriteLine($"✅ [ApiService] Deserialización exitosa: {result != null}");
 System.Diagnostics.Debug.WriteLine($"📊 [ApiService] Tipo resultado: {result?.GetType().Name ?? "NULL"}");
         System.Diagnostics.Debug.WriteLine($"🏁 [ApiService] ===== PETICIÓN HTTP COMPLETADA =====");
  System.Diagnostics.Debug.WriteLine($"");

         return result;
            }
  catch (TaskCanceledException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
          {
    System.Diagnostics.Debug.WriteLine($"");
            System.Diagnostics.Debug.WriteLine($"⏰ [ApiService] ===== TIMEOUT/CONNECTION ERROR =====");
              System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] TaskCanceledException: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] Inner SocketException: {ex.InnerException?.Message}");
          System.Diagnostics.Debug.WriteLine($"🌐 [ApiService] URL que falló: {endpoint}");
                System.Diagnostics.Debug.WriteLine($"⏱️ [ApiService] Timeout configurado: {_httpClient.Timeout.TotalSeconds}s");
        System.Diagnostics.Debug.WriteLine($"");
                throw new Exception("No se pudo conectar al servidor. Verifica tu conexión a internet y que la IP del servidor sea accesible desde tu dispositivo.", ex);
     }
        catch (HttpRequestException ex)
            {
          System.Diagnostics.Debug.WriteLine($"");
     System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] ===== HTTP REQUEST ERROR =====");
        System.Diagnostics.Debug.WriteLine($"❌ [ApiService] HttpRequestException: {ex.Message}");
 System.Diagnostics.Debug.WriteLine($"🌐 [ApiService] URL que falló: {endpoint}");

      if (ex.InnerException != null)
    {
            System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] Inner Exception: {ex.InnerException.GetType().Name}");
        System.Diagnostics.Debug.WriteLine($"💬 [ApiService] Inner Message: {ex.InnerException.Message}");
         }

     System.Diagnostics.Debug.WriteLine($"");
      throw new Exception($"Error de conexión: {ex.Message}. Verifica que el servidor esté ejecutándose en {Endpoints.API_BASE_URL}", ex);
   }
            catch (System.Text.Json.JsonException ex)
          {
             System.Diagnostics.Debug.WriteLine($"");
  System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] ===== JSON DESERIALIZATION ERROR =====");
         System.Diagnostics.Debug.WriteLine($"❌ [ApiService] JsonException: {ex.Message}");
        System.Diagnostics.Debug.WriteLine($"📍 [ApiService] Path: {ex.Path ?? "NULL"}");
          System.Diagnostics.Debug.WriteLine($"📊 [ApiService] LineNumber: {ex.LineNumber}");
         System.Diagnostics.Debug.WriteLine($"📍 [ApiService] BytePositionInLine: {ex.BytePositionInLine}");
       System.Diagnostics.Debug.WriteLine($"");
            throw new Exception($"Error al procesar la respuesta del servidor. La respuesta no es JSON válido.", ex);
         }
catch (Exception ex)
{
         System.Diagnostics.Debug.WriteLine($"");
                System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] ===== UNEXPECTED ERROR =====");
             System.Diagnostics.Debug.WriteLine($"❌ [ApiService] Exception Type: {ex.GetType().Name}");
         System.Diagnostics.Debug.WriteLine($"💬 [ApiService] Message: {ex.Message}");
System.Diagnostics.Debug.WriteLine($"📍 [ApiService] StackTrace: {ex.StackTrace}");
   System.Diagnostics.Debug.WriteLine($"");
  throw new Exception($"Error inesperado: {ex.Message}", ex);
          }
        }

        /// <summary>
        /// Realiza una petición GET y retorna el JSON crudo como string
        /// Útil cuando el formato de respuesta puede variar (array vs objeto)
        /// </summary>
        public async Task<string?> GetRawAsync(string endpoint)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"");
                System.Diagnostics.Debug.WriteLine($"🌐 [ApiService] ===== PETICIÓN GET RAW =====");
                System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] Endpoint: {endpoint}");

                string finalUrl;
                bool isFullUrl = Uri.IsWellFormedUriString(endpoint, UriKind.Absolute);
                
                if (isFullUrl)
                {
                    finalUrl = endpoint;
                }
                else
                {
                    finalUrl = $"{_httpClient.BaseAddress}{endpoint}";
                }

                System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] URL Final: {finalUrl}");

                var response = await _httpClient.GetAsync(finalUrl);
                var content = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📊 [ApiService] Status: {(int)response.StatusCode} - {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"📄 [ApiService] Content Length: {content?.Length ?? 0} chars");

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ [ApiService] Error Response: {content}");
                    throw new Exception($"Error {(int)response.StatusCode}: {content}");
                }

                return content;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [ApiService] GetRawAsync Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Diagnostica la conexión al servidor
        /// </summary>
        public async Task<(bool isConnected, string message)> DiagnoseConnectionAsync()
        {
 try
  {
        System.Diagnostics.Debug.WriteLine($"");
    System.Diagnostics.Debug.WriteLine($"🔍 [ApiService] ===== INICIANDO DIAGNÓSTICO DE CONEXIÓN =====");
    System.Diagnostics.Debug.WriteLine($"📅 [ApiService] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
     System.Diagnostics.Debug.WriteLine("[ApiService] Iniciando diagnóstico de conexión...");
 
     using var testClient = new HttpClient(new HttpClientHandler
      {
         ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
     })
   {
         Timeout = TimeSpan.FromSeconds(10)
 };
   
   // Probar endpoint simple
      var testUrl = $"{Endpoints.API_BASE_URL}security/document-types/";
         System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] URL de prueba: {testUrl}");
   System.Diagnostics.Debug.WriteLine($"⏱️ [ApiService] Timeout diagnóstico: 10 segundos");
     System.Diagnostics.Debug.WriteLine($"🚀 [ApiService] Enviando petición de diagnóstico...");
      
     var startTime = DateTime.Now;
     var response = await testClient.GetAsync(testUrl);
      var endTime = DateTime.Now;
      var duration = endTime - startTime;
          
    System.Diagnostics.Debug.WriteLine($"📥 [ApiService] Respuesta diagnóstico recibida:");
     System.Diagnostics.Debug.WriteLine($"   ⏱️ Duración: {duration.TotalMilliseconds:F0}ms");
   System.Diagnostics.Debug.WriteLine($"   📊 Status: {(int)response.StatusCode} - {response.StatusCode}");
     System.Diagnostics.Debug.WriteLine($"   ✅ Success: {response.IsSuccessStatusCode}");
    
    if (response.IsSuccessStatusCode)
      {
    var successMsg = "Conexión exitosa al servidor";
     System.Diagnostics.Debug.WriteLine($"✅ [ApiService] {successMsg}");
     return (true, successMsg);
    }
    else
   {
     var content = await response.Content.ReadAsStringAsync();
      var errorMsg = $"Servidor respondió con error {response.StatusCode}";
       System.Diagnostics.Debug.WriteLine($"❌ [ApiService] {errorMsg}");
       System.Diagnostics.Debug.WriteLine($"📄 [ApiService] Content: {content}");
         return (false, errorMsg);
       }
            }
    catch (TaskCanceledException ex)
   {
   var timeoutMsg = "Timeout de conexión. Verifica la IP del servidor y tu red.";
    System.Diagnostics.Debug.WriteLine($"⏰ [ApiService] {timeoutMsg}");
    System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] TaskCanceledException: {ex.Message}");
     System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] URL que falló: {Endpoints.API_BASE_URL}security/document-types/");
      return (false, timeoutMsg);
  }
      catch (HttpRequestException ex)
      {
     var networkMsg = $"Error de red: {ex.Message}";
  System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] HttpRequestException: {ex.Message}");
     System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] URL que falló: {Endpoints.API_BASE_URL}security/document-types/");
       
    if (ex.InnerException != null)
      {
         System.Diagnostics.Debug.WriteLine($"🔗 [ApiService] Inner Exception: {ex.InnerException.GetType().Name}");
    System.Diagnostics.Debug.WriteLine($"💬 [ApiService] Inner Message: {ex.InnerException.Message}");
   }
  
 return (false, networkMsg);
       }
   catch (Exception ex)
  {
    var unexpectedMsg = $"Error inesperado: {ex.Message}";
      System.Diagnostics.Debug.WriteLine($"🚨 [ApiService] Exception inesperada: {ex.GetType().Name}");
       System.Diagnostics.Debug.WriteLine($"💬 [ApiService] Message: {ex.Message}");
      System.Diagnostics.Debug.WriteLine($"📍 [ApiService] StackTrace: {ex.StackTrace}");
      return (false, unexpectedMsg);
      }
            finally
   {
     System.Diagnostics.Debug.WriteLine($"🏁 [ApiService] ===== FIN DIAGNÓSTICO DE CONEXIÓN =====");
            System.Diagnostics.Debug.WriteLine($"");
      }
        }

        /// <summary>
        /// Realiza una petición POST con el payload y devuelve la respuesta deserializada
        /// </summary>
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest payload)
      {
         try
            {
           // 🔧 NUEVO: Detectar si el endpoint ya es una URL completa
       bool isFullUrl = Uri.IsWellFormedUriString(endpoint, UriKind.Absolute);
                string logEndpoint = isFullUrl ? endpoint : $"{_httpClient.BaseAddress}{endpoint}";
             
   System.Diagnostics.Debug.WriteLine($"[API] POST Request to: {logEndpoint}");
     System.Diagnostics.Debug.WriteLine($"[API] Endpoint type: {(isFullUrl ? "Full URL" : "Relative")}");
                
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
           
     // 🔧 NUEVO: Usar la URL apropiada según el tipo detectado
                HttpResponseMessage response;
        if (isFullUrl)
        {
                // Para URLs completas, usar Uri
          response = await _httpClient.PostAsync(new Uri(endpoint), content);
     }
          else
       {
      // Para endpoints relativos, usar el método normal
          response = await _httpClient.PostAsync(endpoint, content);
     }
        
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

        /// <summary>
        /// Intenta parsear una respuesta HttpResponseMessage a un objeto que contenga
        /// la información de éxito y detalle, independientemente de cómo el servidor la envíe.
        /// </summary>
        public async Task<AutogestionSena.MAUI.Api.Dtos.ApiResponseParsedDto> ParseApiResponseAsync(HttpResponseMessage response)
        {
            var result = new AutogestionSena.MAUI.Api.Dtos.ApiResponseParsedDto
            {
                StatusCode = (int)response.StatusCode,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                Success = response.IsSuccessStatusCode
            };

            if (response.Content == null)
                return result;

            string content = string.Empty;
            try
            {
                content = await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(content))
                return result;

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(content);
                var root = doc.RootElement;

                // Helper to find property case-insensitively
                bool TryGetPropertyIgnoreCase(System.Text.Json.JsonElement element, string propertyName, out System.Text.Json.JsonElement found)
                {
                    foreach (var p in element.EnumerateObject())
                    {
                        if (string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                        {
                            found = p.Value;
                            return true;
                        }
                    }
                    found = default;
                    return false;
                }

                // Try 'success' field
                if (TryGetPropertyIgnoreCase(root, "success", out var successProp))
                {
                    switch (successProp.ValueKind)
                    {
                        case System.Text.Json.JsonValueKind.True:
                        case System.Text.Json.JsonValueKind.False:
                            result.Success = successProp.GetBoolean();
                            break;
                        case System.Text.Json.JsonValueKind.String:
                            var s = successProp.GetString();
                            if (bool.TryParse(s, out var b)) result.Success = b;
                            break;
                        case System.Text.Json.JsonValueKind.Number:
                            if (successProp.TryGetInt32(out var i)) result.Success = i != 0;
                            break;
                    }
                }

                // Try to obtain a detail/message
                string? GetFirstStringProperty(System.Text.Json.JsonElement element, params string[] names)
                {
                    foreach (var name in names)
                    {
                        if (TryGetPropertyIgnoreCase(element, name, out var prop))
                        {
                            if (prop.ValueKind == System.Text.Json.JsonValueKind.String)
                                return prop.GetString();
                            else
                                return prop.ToString();
                        }
                    }
                    return null;
                }

                var detail = GetFirstStringProperty(root, "detail", "message", "error", "errors", "description");
                if (!string.IsNullOrEmpty(detail)) result.Detail = detail;
                else
                {
                    // If not found but top-level is a string or primitive, take whole content
                    if (root.ValueKind == System.Text.Json.JsonValueKind.String)
                        result.Detail = root.GetString();
                    else
                        result.Detail = content;
                }
            }
            catch (System.Text.Json.JsonException jsonEx)
            {
                // Not a JSON document; default to raw content
                System.Diagnostics.Debug.WriteLine($"[API] ParseApiResponseAsync: Not JSON: {jsonEx.Message}");
                result.Detail = content;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] ParseApiResponseAsync: Error: {ex}");
            }

            return result;
        }
    }
}
