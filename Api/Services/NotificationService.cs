using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSena.MAUI.Api.Services
{
    public class NotificationService
    {
        private readonly ApiService _apiService;

        public NotificationService()
        {
            _apiService = new ApiService();
        }

        public NotificationService(ApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Obtiene las notificaciones según rol y id de usuario
        /// roleQueryName debe ser uno de: apprentice_id, instructor_id, coordinator_id, sofia_operator_id, admin_id
        /// </summary>
        public async Task<List<NotificationDto>?> GetNotificationsAsync(string roleQueryName, int userId)
        {
            try
            {
                var endpoint = $"{Endpoints.Notification.GetNotifications}?{roleQueryName}={userId}";
                return await _apiService.GetAsync<List<NotificationDto>>(endpoint);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotificationService] Error GetNotificationsAsync: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Elimina (desactiva) una notificación por id
        /// </summary>
        public async Task<bool> DeleteNotificationByIdAsync(int notificationId)
        {
            try
            {
                var endpoint = $"{Endpoints.Notification.DeleteById}?id={notificationId}";
                var resp = await _apiService.DeleteAsync(endpoint);
                return resp.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotificationService] Error DeleteNotificationByIdAsync: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Desactiva (elimina) todas las notificaciones de un usuario según rol
        /// roleQueryName debe ser admin_id, apprentice_id, etc.
        /// </summary>
        public async Task<bool> DeleteNotificationsByUserAsync(string roleQueryName, int userId)
        {
            try
            {
                var endpoint = $"{Endpoints.Notification.DeleteByUser}?{roleQueryName}={userId}";
                var resp = await _apiService.DeleteAsync(endpoint);
                return resp.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotificationService] Error DeleteNotificationsByUserAsync: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Marca como leída una notificación solicitando GET al recurso /general/notifications/{id}/
        /// </summary>
        public async Task<NotificationDto?> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var endpoint = Endpoints.Notification.GetById(notificationId);
                return await _apiService.GetAsync<NotificationDto>(endpoint);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NotificationService] Error MarkAsReadAsync: {ex}");
                throw;
            }
        }
    }
}
