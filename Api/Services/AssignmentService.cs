using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.Api.Services
{
    /// <summary>
    /// Servicio para manejar las solicitudes de asignación
    /// </summary>
    public class AssignmentService
    {
        private readonly ApiService _apiService;

        public AssignmentService()
        {
            _apiService = new ApiService();
        }

        public AssignmentService(ApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Obtiene todas las solicitudes de asignación
        /// </summary>
        public async Task<AssignmentRequestListResponse?> GetFormRequestListAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[AssignmentService] Obteniendo lista de solicitudes de asignación...");
                var response = await _apiService.GetAsync<AssignmentRequestListResponse>(Endpoints.Assignment.GetFormRequestList);
                
                if (response != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[AssignmentService] ✅ {response.Count} solicitudes encontradas");
                    
                    if (response.Data != null && response.Data.Any())
                    {
                        var sinAsignar = response.Data.Count(x => x.RequestState == "SIN_ASIGNAR");
                        System.Diagnostics.Debug.WriteLine($"[AssignmentService]   - Sin asignar: {sinAsignar}");
                        System.Diagnostics.Debug.WriteLine($"[AssignmentService]   - Total: {response.Count}");
                    }
                }
                
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AssignmentService] ❌ Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Obtiene el conteo de solicitudes sin asignar
        /// </summary>
        public async Task<int> GetUnassignedCountAsync()
        {
            try
            {
                var response = await GetFormRequestListAsync();
                
                if (response?.Data == null)
                    return 0;

                return response.Data.Count(x => x.RequestState == "SIN_ASIGNAR");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AssignmentService] ❌ Error obteniendo conteo sin asignar: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Obtiene todas las solicitudes que están sin asignar
        /// </summary>
        public async Task<List<AssignmentRequestDto>?> GetUnassignedRequestsAsync()
        {
            try
            {
                var response = await GetFormRequestListAsync();
                
                if (response?.Data == null)
                    return new List<AssignmentRequestDto>();

                return response.Data.Where(x => x.RequestState == "SIN_ASIGNAR").ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AssignmentService] ❌ Error obteniendo solicitudes sin asignar: {ex.Message}");
                return new List<AssignmentRequestDto>();
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
