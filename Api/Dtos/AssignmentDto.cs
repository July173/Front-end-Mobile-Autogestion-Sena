using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutogestionSena.MAUI.Api.Dtos
{
    /// <summary>
    /// DTO para solicitudes de asignación
    /// </summary>
    public class AssignmentRequestDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("aprendiz_id")]
        public int AprendizId { get; set; }

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [JsonPropertyName("tipo_identificacion")]
        public int TipoIdentificacion { get; set; }

        [JsonPropertyName("numero_identificacion")]
        public int NumeroIdentificacion { get; set; }

        [JsonPropertyName("fecha_solicitud")]
        public string? FechaSolicitud { get; set; }

        /// <summary>
        /// Fecha de creación parseada como DateTime
        /// </summary>
        public DateTime? CreatedAt
        {
            get
            {
                if (string.IsNullOrEmpty(FechaSolicitud))
                    return null;
                
                if (DateTime.TryParse(FechaSolicitud, out DateTime result))
                    return result;
                
                return null;
            }
        }

        [JsonPropertyName("request_state")]
        public string? RequestState { get; set; }

        [JsonPropertyName("nombre_modalidad")]
        public string? NombreModalidad { get; set; }
    }

    /// <summary>
    /// Respuesta del endpoint de lista de solicitudes
    /// </summary>
    public class AssignmentRequestListResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("data")]
        public List<AssignmentRequestDto>? Data { get; set; }
    }

    /// <summary>
    /// DTO simple de aprendiz (solo lo básico)
    /// </summary>
    public class ApprenticeSimpleDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("person")]
        public int Person { get; set; }

        [JsonPropertyName("ficha")]
        public int? Ficha { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
