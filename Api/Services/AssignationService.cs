using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api;

namespace AutogestionSena.MAUI.Api.Services
{
    public class AssignationService
    {
        private readonly ApiService _apiService;
        public AssignationService() => _apiService = new ApiService();

        /// <summary>
        /// Obtiene el dashboard del aprendiz
        /// Endpoint: GET assign/request_asigation/aprendiz-dashboard/?aprendiz_id={id}
        /// NOTA: El API devuelve DOS formatos diferentes:
        /// 1. Array [{}] cuando NO hay instructor asignado (PRE-APROBADO)
        /// 2. Objeto {} cuando SÍ hay instructor asignado (VERIFICANDO, etc.)
        /// </summary>
        public async Task<ApprenticeDashboardDto> GetApprenticeDashboardAsync(int apprenticeId)
        {
            try
            {
                var endpoint = $"assign/request_asignation/aprendiz-dashboard/?aprendiz_id={apprenticeId}";
  
                System.Diagnostics.Debug.WriteLine($"");
                System.Diagnostics.Debug.WriteLine($"🎯 [AssignationService] ===== INICIANDO PETICIÓN DASHBOARD =====");
                System.Diagnostics.Debug.WriteLine($"📅 [AssignationService] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                System.Diagnostics.Debug.WriteLine($"📋 [AssignationService] Endpoint: {endpoint}");
                System.Diagnostics.Debug.WriteLine($"👤 [AssignationService] Apprentice ID: {apprenticeId}");

                // Primero obtener el JSON raw para detectar el tipo de respuesta
                var jsonResponse = await _apiService.GetRawAsync(endpoint);
                
                if (string.IsNullOrEmpty(jsonResponse))
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [AssignationService] Respuesta vacía del servidor");
                    return CreateEmptyDashboard();
                }

                System.Diagnostics.Debug.WriteLine($"📄 [AssignationService] JSON recibido: {jsonResponse.Substring(0, Math.Min(200, jsonResponse.Length))}...");

                // Detectar si es array o objeto
                var trimmedJson = jsonResponse.Trim();
                
                if (trimmedJson.StartsWith("["))
                {
                    // Es un ARRAY - formato sin instructor (PRE-APROBADO, etc.)
                    System.Diagnostics.Debug.WriteLine($"📦 [AssignationService] Detectado formato ARRAY (sin instructor)");
                    return await ProcessArrayResponse(trimmedJson);
                }
                else if (trimmedJson.StartsWith("{"))
                {
                    // Es un OBJETO - formato con instructor (VERIFICANDO, etc.)
                    System.Diagnostics.Debug.WriteLine($"📦 [AssignationService] Detectado formato OBJETO (con instructor)");
                    return await ProcessObjectResponse(trimmedJson);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Formato de respuesta desconocido");
                    return CreateEmptyDashboard();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"");
                System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] ===== ERROR EN PETICIÓN =====");
                System.Diagnostics.Debug.WriteLine($"🚨 [AssignationService] Exception Type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"💬 [AssignationService] Error Message: {ex.Message}");
                
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"🔗 [AssignationService] Inner Exception: {ex.InnerException.Message}");
                }
                
                System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] ===== FIN ERROR =====");
                throw;
            }
        }

        /// <summary>
        /// Procesa la respuesta cuando es un ARRAY (sin instructor asignado)
        /// </summary>
        private async Task<ApprenticeDashboardDto> ProcessArrayResponse(string json)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var responses = JsonSerializer.Deserialize<List<ApprenticeDashboardBasicApiResponse>>(json, options);
                
                if (responses == null || responses.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [AssignationService] Array vacío - sin solicitudes");
                    return CreateEmptyDashboard();
                }

                // Tomar la primera solicitud (o la más reciente si hay varias)
                var response = responses.FirstOrDefault();
                
                if (response == null)
                {
                    return CreateEmptyDashboard();
                }

                System.Diagnostics.Debug.WriteLine($"✅ [AssignationService] Procesando solicitud básica ID: {response.Id}");
                System.Diagnostics.Debug.WriteLine($"   📊 Estado: {response.RequestState}");
                System.Diagnostics.Debug.WriteLine($"   🏢 Enterprise ID: {response.Enterprise}");
                System.Diagnostics.Debug.WriteLine($"   📋 Modality ID: {response.ModalityProductiveStage}");

                return await MapBasicDashboardResponse(response);
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error deserializando array: {ex.Message}");
                return CreateEmptyDashboard();
            }
        }

        /// <summary>
        /// Procesa la respuesta cuando es un OBJETO (con instructor asignado)
        /// </summary>
        private async Task<ApprenticeDashboardDto> ProcessObjectResponse(string json)
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = JsonSerializer.Deserialize<ApprenticeDashboardRealApiResponse>(json, options);
                
                if (response == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ [AssignationService] Objeto null después de deserializar");
                    return CreateEmptyDashboard();
                }

                System.Diagnostics.Debug.WriteLine($"✅ [AssignationService] Procesando solicitud con instructor ID: {response.Id}");
                System.Diagnostics.Debug.WriteLine($"   📊 Estado: {response.RequestState}");
                System.Diagnostics.Debug.WriteLine($"   👨‍🏫 Instructor: {response.InstructorFirstName} {response.InstructorFirstLastName}");

                return await MapRealDashboardResponse(response);
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error deserializando objeto: {ex.Message}");
                return CreateEmptyDashboard();
            }
        }

        /// <summary>
        /// Crea un dashboard vacío cuando no hay datos
        /// </summary>
        private ApprenticeDashboardDto CreateEmptyDashboard()
        {
            return new ApprenticeDashboardDto
            {
                HasRequest = false,
                RequestState = "Sin solicitudes registradas",
                ShowInstructor = false
            };
        }

        /// <summary>
        /// Mapea la respuesta básica (sin instructor) al DTO del dashboard
        /// </summary>
        private async Task<ApprenticeDashboardDto> MapBasicDashboardResponse(ApprenticeDashboardBasicApiResponse response)
        {
            System.Diagnostics.Debug.WriteLine($"📦 [AssignationService] ===== MAPEANDO RESPUESTA BÁSICA =====");

            // Obtener datos de empresa
            EnterpriseDto? enterprise = null;
            if (response.Enterprise > 0)
            {
                try
                {
                    enterprise = await GetEnterpriseAsync(response.Enterprise);
                    System.Diagnostics.Debug.WriteLine($"🏢 [AssignationService] Empresa: {enterprise?.Name ?? "NULL"}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error empresa: {ex.Message}");
                }
            }

            // Obtener datos de modalidad
            ModalityProductiveStageDto? modality = null;
            if (response.ModalityProductiveStage > 0)
            {
                try
                {
                    modality = await GetModalityByIdAsync(response.ModalityProductiveStage);
                    System.Diagnostics.Debug.WriteLine($"📋 [AssignationService] Modalidad: {modality?.Name ?? "NULL"}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error modalidad: {ex.Message}");
                }
            }

            var request = new RequestDto
            {
                Id = response.Id,
                EnterpriseName = enterprise?.Name ?? "Empresa no encontrada",
                BossName = enterprise?.ImmediateBoss ?? "Pendiente de asignación",
                Modality = modality?.Name ?? "N/A",
                StartDate = response.StartDate ?? response.DateStartProductionStage,
                EndDate = response.EndDate,
                RequestDate = response.RequestDate,
                RequestState = response.RequestState ?? "PENDIENTE",
                PdfUrl = response.PdfUrl ?? response.PdfRequest,
                CityName = enterprise?.Address ?? "N/A",
                StateDisplay = GetStateDisplay(response.RequestState),
                StateColor = GetStateColor(response.RequestState),
                StateCode = response.RequestState
            };

            System.Diagnostics.Debug.WriteLine($"📝 [AssignationService] Request mapeado:");
            System.Diagnostics.Debug.WriteLine($"   🏢 Empresa: {request.EnterpriseName}");
            System.Diagnostics.Debug.WriteLine($"   📋 Modalidad: {request.Modality}");
            System.Diagnostics.Debug.WriteLine($"   🎯 Estado: {request.RequestState}");
            System.Diagnostics.Debug.WriteLine($"   👨‍🏫 Instructor: NO ASIGNADO");

            return new ApprenticeDashboardDto
            {
                HasRequest = true,
                Request = request,
                Instructor = null, // Sin instructor en este estado
                RequestState = request.RequestState,
                ShowInstructor = false
            };
        }

    /// <summary>
    /// Mapea la respuesta real del API al DTO del dashboard
/// </summary>
     private async Task<ApprenticeDashboardDto> MapRealDashboardResponse(ApprenticeDashboardRealApiResponse? response)
     {
          System.Diagnostics.Debug.WriteLine($"📦 [AssignationService] ===== INICIANDO MAPEO REAL =====");
          
  const string defaultState = "Sin solicitudes registradas";

      if (response == null)
            {
  System.Diagnostics.Debug.WriteLine($"⚠️ [AssignationService] Response es null - Retornando estado vacío");
 return new ApprenticeDashboardDto
          {
    HasRequest = false,
         RequestState = defaultState,
        ShowInstructor = false
         };
            }

     System.Diagnostics.Debug.WriteLine($"✅ [AssignationService] Mapeando datos reales...");
      
      // Obtener datos adicionales de empresa si es necesario
        EnterpriseDto? enterprise = null;
        if (response.Enterprise > 0)
      {
            try
       {
       System.Diagnostics.Debug.WriteLine($"🏢 [AssignationService] Obteniendo datos de empresa ID: {response.Enterprise}");
       enterprise = await GetEnterpriseAsync(response.Enterprise);
       System.Diagnostics.Debug.WriteLine($"🏢 [AssignationService] Empresa obtenida: {enterprise?.Name ?? "NULL"}");
       }
     catch (Exception ex)
        {
       System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error obteniendo empresa: {ex.Message}");
  }
    }

      // Obtener datos adicionales de modalidad si es necesario
      ModalityProductiveStageDto? modality = null;
      if (response.ModalityProductiveStage > 0)
      {
          try
          {
              System.Diagnostics.Debug.WriteLine($"📋 [AssignationService] Obteniendo modalidad ID: {response.ModalityProductiveStage}");
              modality = await GetModalityByIdAsync(response.ModalityProductiveStage);
              System.Diagnostics.Debug.WriteLine($"📋 [AssignationService] Modalidad obtenida: {modality?.Name ?? "NULL"}");
          }
          catch (Exception ex)
          {
              System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error obteniendo modalidad: {ex.Message}");
          }
      }

      // Crear el DTO de solicitud
    var request = new RequestDto
       {
Id = response.Id,
            EnterpriseName = enterprise?.Name ?? "Empresa no encontrada",
       BossName = enterprise?.ImmediateBoss ?? "No especificado",
         Modality = modality?.Name ?? "N/A",
        StartDate = response.StartDate,
       EndDate = response.EndDate,
  RequestDate = response.RequestDate,
          RequestState = response.RequestState ?? defaultState,
    PdfUrl = response.PdfUrl,
      CityName = enterprise?.Address ?? "N/A",
    StateDisplay = response.RequestState ?? defaultState,
             StateColor = GetStateColor(response.RequestState),
StateCode = response.RequestState
      };

      System.Diagnostics.Debug.WriteLine($"📝 [AssignationService] Request creado:");
      System.Diagnostics.Debug.WriteLine($"   🏢 EnterpriseName: {request.EnterpriseName}");
      System.Diagnostics.Debug.WriteLine($"   👔 BossName: {request.BossName}");
      System.Diagnostics.Debug.WriteLine($"   📋 Modality: {request.Modality}");
      System.Diagnostics.Debug.WriteLine($"   📍 CityName: {request.CityName}");
      System.Diagnostics.Debug.WriteLine($"   🎯 RequestState: {request.RequestState}");

    // Crear el DTO de instructor si existe
 DashboardInstructorDto? instructor = null;
    if (response.InstructorId.HasValue && response.InstructorId > 0)
 {
   instructor = new DashboardInstructorDto
     {
      Id = response.InstructorId.Value,
      FirstName = response.InstructorFirstName,
      SecondName = response.InstructorSecondName,
  FirstLastName = response.InstructorFirstLastName,
    SecondLastName = response.InstructorSecondLastName,
       Email = response.InstructorEmail,
   Phone = response.InstructorPhoneNumber?.ToString(),
   KnowledgeArea = response.InstructorKnowledgeArea,
     AssignedAt = DateTime.Now.ToString("yyyy-MM-dd"), // Temporal
  ShowContact = true
      };
 }

   var result = new ApprenticeDashboardDto
     {
          HasRequest = true,
     Request = request,
      Instructor = instructor,
    RequestState = request.RequestState,
         ShowInstructor = instructor != null
     };

         System.Diagnostics.Debug.WriteLine($"✅ [AssignationService] Mapeo real completado exitosamente");
            return result;
        }

        /// <summary>
        /// Obtiene una modalidad por ID de la lista completa
        /// </summary>
        private async Task<ModalityProductiveStageDto?> GetModalityByIdAsync(int modalityId)
        {
            try
            {
                var modalities = await GetModalityProductiveStagesAsync();
                return modalities?.FirstOrDefault(m => m.Id == modalityId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [AssignationService] Error buscando modalidad {modalityId}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene el color del estado según el código
        /// Estados del sistema:
        /// - RECHAZADO: cuando el coordinador rechaza la solicitud
        /// - ASIGNADO: cuando el coordinador asigna un instructor para seguimiento
        /// - ASIGNAR: cuando la solicitud aún no tiene instructor asignado y está pendiente
        /// - VERIFICANDO: cuando se ha asignado instructor para valoración pero no la ha realizado
        /// - PRE-APROBADO: cuando el instructor ya realizó la valoración y el coordinador debe decidir
        /// </summary>
        private static string GetStateColor(string? state)
        {
            return state?.ToUpper() switch
            {
                "RECHAZADO" => "#EF4444",      // Rojo - Solicitud rechazada
                "ASIGNADO" => "#10B981",       // Verde - Instructor asignado para seguimiento
                "ASIGNAR" => "#F59E0B",        // Amarillo - Pendiente de asignar instructor
                "VERIFICANDO" => "#3B82F6",   // Azul - Instructor valorando
                "PRE-APROBADO" => "#8B5CF6",  // Púrpura - Valoración realizada, esperando coordinador
                "APROBADO" => "#10B981",       // Verde - Aprobado
                "EN_PROCESO" => "#3B82F6",     // Azul - En seguimiento
                "FINALIZADO" => "#6B7280",     // Gris - Terminado
                "CANCELADO" => "#EF4444",      // Rojo - Cancelado
                _ => "#CBD5E1"                 // Gris claro por defecto
            };
        }

        /// <summary>
        /// Obtiene el texto legible del estado
        /// </summary>
        private static string GetStateDisplay(string? state)
        {
            return state?.ToUpper() switch
            {
                "RECHAZADO" => "Rechazado",
                "ASIGNADO" => "Instructor Asignado",
                "ASIGNAR" => "Pendiente de Asignación",
                "VERIFICANDO" => "En Verificación",
                "PRE-APROBADO" => "Pre-aprobado (Esperando Decisión)",
                "APROBADO" => "Aprobado",
                "EN_PROCESO" => "En Proceso",
                "FINALIZADO" => "Finalizado",
                "CANCELADO" => "Cancelado",
                _ => state ?? "Pendiente"
            };
        }

        /// <summary>
    /// Obtiene los datos de una empresa por ID
  /// Endpoint: GET assign/enterprise/{id}/
   /// </summary>
        public async Task<EnterpriseDto?> GetEnterpriseAsync(int enterpriseId)
    {
     try
   {
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Obteniendo datos de empresa ID: {enterpriseId}");
      var endpoint = Endpoints.Assignment.GetEnterprise(enterpriseId);
 return await _apiService.GetAsync<EnterpriseDto>(endpoint);
       }
    catch (Exception ex)
     {
      System.Diagnostics.Debug.WriteLine($"? [AssignationService] Error obteniendo empresa: {ex.Message}");
        throw;
         }
        }

        /// <summary>
    /// Obtiene la lista de modalidades de etapa productiva
 /// Endpoint: GET assign/modality_productive_stage/
     /// </summary>
     public async Task<List<ModalityProductiveStageDto>?> GetModalityProductiveStagesAsync()
  {
          try
         {
  System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Obteniendo modalidades de etapa productiva");
    var endpoint = Endpoints.Assignment.GetModalityProductiveStage;
      return await _apiService.GetAsync<List<ModalityProductiveStageDto>>(endpoint);
    }
 catch (Exception ex)
            {
     System.Diagnostics.Debug.WriteLine($"? [AssignationService] Error obteniendo modalidades: {ex.Message}");
      throw;
  }
        }
   
     /// <summary>
        /// Obtiene los datos completos de un instructor por ID
/// Endpoint: GET general/instructors/{id}/
        /// </summary>
 public async Task<InstructorDetailDto?> GetInstructorDetailAsync(int instructorId)
        {
 try
        {
     System.Diagnostics.Debug.WriteLine($"????? [AssignationService] Obteniendo datos de instructor ID: {instructorId}");
          var endpoint = Endpoints.Instructor.GetInstructor(instructorId);
   return await _apiService.GetAsync<InstructorDetailDto>(endpoint);
     }
          catch (Exception ex)
    {
     System.Diagnostics.Debug.WriteLine($"? [AssignationService] Error obteniendo instructor: {ex.Message}");
  throw;
         }
        }
    }
}
