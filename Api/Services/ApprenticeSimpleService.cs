using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.Api.Services
{
    /// <summary>
    /// Servicio simple para obtener lista de aprendices
    /// </summary>
    public class ApprenticeSimpleService
    {
        private readonly ApiService _apiService;

        public ApprenticeSimpleService()
        {
            _apiService = new ApiService();
        }

        public ApprenticeSimpleService(ApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Obtiene todos los aprendices (lista simple)
        /// </summary>
        public async Task<List<ApprenticeSimpleDto>?> GetAllApprenticesAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ApprenticeService] Obteniendo lista de aprendices...");
                var response = await _apiService.GetAsync<List<ApprenticeSimpleDto>>(Endpoints.ApprenticeSimple.GetAllApprenticesSimple);
                
                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[ApprenticeService] ✅ {response.Count} aprendices encontrados");
                    
                    var activos = response.Count(x => x.Active);
                    System.Diagnostics.Debug.WriteLine($"[ApprenticeService]   - Activos: {activos}");
                    System.Diagnostics.Debug.WriteLine($"[ApprenticeService]   - Total: {response.Count}");
                }
                
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApprenticeService] ❌ Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene el conteo de aprendices activos
        /// </summary>
        public async Task<int> GetActiveApprenticesCountAsync()
        {
            try
            {
                var response = await GetAllApprenticesAsync();
                
                if (response == null)
                    return 0;

                return response.Count(x => x.Active);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApprenticeService] ❌ Error obteniendo conteo: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Configura el token de autenticación
        /// </summary>
        public void SetAuthToken(string token)
        {
            _apiService.SetAuthToken(token);
        }
    }
}
