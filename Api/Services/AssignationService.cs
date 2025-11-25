using System;
using System.Linq;
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
        /// </summary>
        public async Task<ApprenticeDashboardDto> GetApprenticeDashboardAsync(int apprenticeId)
        {
            try
            {
                // ?? CORREGIDO: Usar el par�metro correcto 'aprendiz_id'
                var endpoint = $"assign/request_asignation/aprendiz-dashboard/?aprendiz_id={apprenticeId}";
  
                // ?? LOG DE DEPURACI�N - URL COMPLETA
     System.Diagnostics.Debug.WriteLine($"");
      System.Diagnostics.Debug.WriteLine($"?? [AssignationService] ===== INICIANDO PETICI�N DASHBOARD =====");
     System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
      System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Endpoint relativo: {endpoint}");
System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Apprentice ID: {apprenticeId}");
        System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Base URL: {Endpoints.API_BASE_URL}");
       System.Diagnostics.Debug.WriteLine($"?? [AssignationService] URL final que se construir�: {Endpoints.API_BASE_URL}{endpoint}");
     
#if ANDROID
                System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Plataforma: Android");
#elif IOS
          System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Plataforma: iOS");
#else
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Plataforma: Windows");
#endif
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Iniciando petici�n HTTP GET...");
      
    // ?? NUEVO: Usar la estructura real del API
       var response = await _apiService.GetAsync<ApprenticeDashboardRealApiResponse>(endpoint);
      
 // ?? LOG DE DEPURACI�N - RESPUESTA RECIBIDA
        System.Diagnostics.Debug.WriteLine($"");
 System.Diagnostics.Debug.WriteLine($"? [AssignationService] ===== RESPUESTA RECIBIDA =====");
  System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Response != null: {response != null}");
    
    if (response != null)
    {
   System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Response.Id: {response.Id}");
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Enterprise ID: {response.Enterprise}");
         System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Modality ID: {response.ModalityProductiveStage}");
          System.Diagnostics.Debug.WriteLine($"?? [AssignationService] State: {response.RequestState ?? "NULL"}");
System.Diagnostics.Debug.WriteLine($"?? [AssignationService] PDF URL: {response.PdfUrl ?? "NULL"}");
         System.Diagnostics.Debug.WriteLine($"????? [AssignationService] Instructor ID: {response.InstructorId}");
    System.Diagnostics.Debug.WriteLine($"????? [AssignationService] Instructor Name: {response.InstructorFirstName} {response.InstructorFirstLastName}");
 }
   else
      {
      System.Diagnostics.Debug.WriteLine($"? [AssignationService] Response es NULL - Sin datos del servidor");
 }
 
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Iniciando mapeo de respuesta...");
  var mappedResult = await MapRealDashboardResponse(response);
        
   System.Diagnostics.Debug.WriteLine($"");
      System.Diagnostics.Debug.WriteLine($"? [AssignationService] ===== RESULTADO FINAL =====");
 System.Diagnostics.Debug.WriteLine($"?? [AssignationService] MappedResult != null: {mappedResult != null}");
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] HasRequest: {mappedResult?.HasRequest}");
        System.Diagnostics.Debug.WriteLine($"?? [AssignationService] RequestState: {mappedResult?.RequestState ?? "NULL"}");
           System.Diagnostics.Debug.WriteLine($"????? [AssignationService] ShowInstructor: {mappedResult?.ShowInstructor}");
     System.Diagnostics.Debug.WriteLine($"?? [AssignationService] ===== PETICI�N COMPLETADA =====");
System.Diagnostics.Debug.WriteLine($"");
    
      return mappedResult;
    }
   catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"");
   System.Diagnostics.Debug.WriteLine($"? [AssignationService] ===== ERROR EN PETICI�N =====");
        System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Exception Type: {ex.GetType().Name}");
      System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Error Message: {ex.Message}");
    System.Diagnostics.Debug.WriteLine($"?? [AssignationService] StackTrace: {ex.StackTrace}");
 
      if (ex.InnerException != null)
    {
       System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Inner Exception: {ex.InnerException.Message}");
  System.Diagnostics.Debug.WriteLine($"?? [AssignationService] Inner StackTrace: {ex.InnerException.StackTrace}");
    }
      
         System.Diagnostics.Debug.WriteLine($"?? [AssignationService] ===== FIN ERROR =====");
     System.Diagnostics.Debug.WriteLine($"");
        throw;
 }
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
       BossName = "N/A", // No viene en la respuesta
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
        /// Obtiene el color del estado seg�n el c�digo
 /// </summary>
    private static string GetStateColor(string? state)
 {
         return state?.ToUpper() switch
         {
  "SIN_ASIGNAR" => "#F59E0B", // Amarillo
      "ASIGNADO" => "#10B981", // Verde
    "EN_PROCESO" => "#3B82F6", // Azul
      "FINALIZADO" => "#6B7280", // Gris
    "CANCELADO" => "#EF4444", // Rojo
    _ => "#CBD5E1" // Gris claro por defecto
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
