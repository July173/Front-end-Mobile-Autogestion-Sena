using System;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;
using System.Collections.Generic;

namespace AutogestionSena.MAUI.Api.Services
{
    public class ApprenticeService
    {
        private readonly ApiService _apiService;
        public ApprenticeService() => _apiService = new ApiService();

        public async Task<ApprenticeDto?> GetApprenticeByPersonAsync(int personId)
        {
            try
            {
                // endpoint to get apprentices by person might be: general/aprendices/?person={id}
                var url = $"general/aprendices/?person={personId}";
                var response = await _apiService.GetAsync<List<ApprenticeDto>>(url);
                return response is { Count: > 0 } ? response[0] : null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApprenticeService] Error GetApprenticeByPersonAsync: {ex}");
                throw;
            }
        }
    }
}
