using System;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.Api.Services
{
    public class AssignationService
    {
        private readonly ApiService _apiService;
        public AssignationService() => _apiService = new ApiService();

        /// <summary>
        /// Obtiene el dashboard del aprendiz
        /// Endpoint: GET assign/request_asignation/aprendiz-dashboard/?apprentice_id={id}
        /// </summary>
        public async Task<ApprenticeDashboardDto?> GetApprenticeDashboardAsync(int apprenticeId)
        {
            try
            {
                var url = $"assign/request_asignation/aprendiz-dashboard/?apprentice_id={apprenticeId}";
                return await _apiService.GetAsync<ApprenticeDashboardDto>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AssignationService] Error GetApprenticeDashboardAsync: {ex}");
                throw;
            }
        }
    }
}
