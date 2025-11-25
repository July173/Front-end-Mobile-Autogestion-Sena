using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using AutogestionSena.MAUI.Api.Dtos;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api;

namespace AutogestionSenaMaui.ViewModels
{
    public class ApprenticeDashboardViewModel : BindableObject
    {
        private readonly AssignationService _assignService;
        private readonly ApiService _apiService;
        private bool _isLoading;
        private ApprenticeDashboardDto? _dashboard;
        private int _apprenticesCount;
        private int _unassignedRequestsCount;
        private int _assignedRequestsCount;
        private string _apprenticeName = "Aprendiz";
        private string _diagnosticMessage = "";
        private bool _showDiagnosticMessage;

        public ApprenticeDashboardViewModel()
        {
            _assignService = new AssignationService();
            _apiService = new ApiService();
            RefreshCommand = new Command(async () => await LoadAsync(ApprenticeId));
            OpenPdfCommand = new Command(async () => await OpenPdfAsync());
            DiagnoseConnectionCommand = new Command(async () => await DiagnoseConnectionAsync());
            LoadApprenticeMetadata();
        }

        public int ApprenticesCount
        {
            get => _apprenticesCount;
            set { _apprenticesCount = value; OnPropertyChanged(); }
        }

        public int UnassignedRequestsCount
        {
            get => _unassignedRequestsCount;
            set { _unassignedRequestsCount = value; OnPropertyChanged(); }
        }

        public int AssignedRequestsCount
        {
            get => _assignedRequestsCount;
            set { _assignedRequestsCount = value; OnPropertyChanged(); }
        }

        public int ApprenticeId { get; set; }

        public ApprenticeDashboardDto? Dashboard
        {
            get => _dashboard;
            set
            {
                _dashboard = value;
                OnPropertyChanged();
                RaiseDashboardDerivedProperties();
            }
        }

        public string DiagnosticMessage
        {
            get => _diagnosticMessage;
            set { _diagnosticMessage = value; OnPropertyChanged(); }
        }

        public bool ShowDiagnosticMessage
        {
            get => _showDiagnosticMessage;
            set { _showDiagnosticMessage = value; OnPropertyChanged(); }
        }

        public bool HasRequest => Dashboard?.HasRequest ?? false;

        public bool ShowRequestDetail => Dashboard?.HasRequest ?? false;

        public bool ShowEmptyRequestState => !ShowRequestDetail;

        public string ApprenticeName
        {
            get => _apprenticeName;
            set { _apprenticeName = value; OnPropertyChanged(); }
        }

        public string RequestStateLabel => Dashboard?.Request?.StateDisplay ?? Dashboard?.RequestState ?? "Sin solicitudes registradas";

        public string RequestStateColor => Dashboard?.Request?.StateColor ?? "#B8B8B8";

        public string RequestStateDescription => HasRequest
            ? $"Tu solicitud se encuentra en estado {RequestStateLabel}."
            : "Aún no has solicitado ningún proceso para tu etapa productiva. Por favor registra una solicitud.";

        public string RequestActionText => HasRequest ? "Ver solicitud" : "Hacer una solicitud";

        public string EnterpriseName => Dashboard?.Request?.EnterpriseName ?? "Sin empresa registrada";

        public string BossName => Dashboard?.Request?.BossName ?? "Sin jefe registrado";

        public string ModalityDisplay => Dashboard?.Request?.Modality ?? "Sin modalidad";

        public string RequestDateDisplay => FormatDate(Dashboard?.Request?.RequestDate);

        public string StartDateDisplay => FormatDate(Dashboard?.Request?.StartDate);

        public string EndDateDisplay => FormatDate(Dashboard?.Request?.EndDate);

        public string CityDisplay => Dashboard?.Request?.CityName ?? "Ciudad no registrada";

        public bool ShowPdfButton => Dashboard?.Request?.HasPdf ?? false;

        public string RequestPdfUrl => Dashboard?.Request?.PdfUrl ?? string.Empty;

        public string InstructorFullName
        {
            get
            {
                var instructor = Dashboard?.Instructor;
                if (instructor == null)
                {
                    return "Instructor no asignado";
                }

                var fullName = $"{instructor.FirstName} {instructor.SecondName ?? string.Empty} {instructor.FirstLastName} {instructor.SecondLastName ?? string.Empty}".Trim();
                return string.IsNullOrWhiteSpace(fullName) ? "Instructor asignado" : fullName;
            }
        }

        public bool HasInstructor => Dashboard?.Instructor != null;

        public bool ShowInstructorCard => Dashboard?.Instructor != null;

        public bool ShowInstructorContact => Dashboard?.ShowInstructor ?? false;

        public string InstructorKnowledgeArea => Dashboard?.Instructor?.KnowledgeArea ?? "Área no registrada";

        public string InstructorContactEmail => Dashboard?.Instructor?.Email ?? "Sin correo";

        public string InstructorContactPhone => Dashboard?.Instructor?.Phone ?? "Sin teléfono";

        public string InstructorAssignedAtDisplay => FormatDate(Dashboard?.Instructor?.AssignedAt);

        public string InstructorTagText => ShowInstructorCard ? "Asignado" : "Pendiente de asignar";

        public string InstructorTagBackground => ShowInstructorCard ? "#DCFCE7" : "#E5E7EB";

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }

        public ICommand OpenPdfCommand { get; }

        public ICommand DiagnoseConnectionCommand { get; }

        public async Task LoadAsync(int apprenticeId)
        {
            try
            {
                IsLoading = true;
                ShowDiagnosticMessage = false;
    
                // 🔍 LOGS DE INICIO
                    Debug.WriteLine($"");
            Debug.WriteLine($"🎯 [ApprenticeDashboardVM] ===== INICIANDO CARGA DASHBOARD =====");
            Debug.WriteLine($"📅 [ApprenticeDashboardVM] Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            Debug.WriteLine($"👤 [ApprenticeDashboardVM] Apprentice ID: {apprenticeId}");
            Debug.WriteLine($"🔄 [ApprenticeDashboardVM] IsLoading: {IsLoading}");
              
    #if ANDROID
                    Debug.WriteLine($"📱 [ApprenticeDashboardVM] Plataforma: Android");
    #elif IOS
                    Debug.WriteLine($"📱 [ApprenticeDashboardVM] Plataforma: iOS");
    #else
          Debug.WriteLine($"💻 [ApprenticeDashboardVM] Plataforma: Windows");
    #endif
     
            Debug.WriteLine($"🌐 [ApprenticeDashboardVM] URL Base: {Endpoints.API_BASE_URL}");
             Debug.WriteLine($"🔗 [ApprenticeDashboardVM] URL Completa que se va a llamar: {Endpoints.Assignment.GetApprenticeDashboard(apprenticeId)}");
           
             Debug.WriteLine($"🔍 [ApprenticeDashboardVM] Iniciando diagnóstico de conectividad...");
                  
                 // Verificar conectividad primero
               var (isConnected, message) = await _apiService.DiagnoseConnectionAsync();
              
             Debug.WriteLine($"📊 [ApprenticeDashboardVM] Resultado diagnóstico:");
              Debug.WriteLine($"   ✅ Conectado: {isConnected}");
            Debug.WriteLine($"   💬 Mensaje: {message}");
            
             if (!isConnected)
                    {
          DiagnosticMessage = $"⚠️ Problema de conexión: {message}";
          ShowDiagnosticMessage = true;
           Debug.WriteLine($"❌ [ApprenticeDashboardVM] Diagnóstico fallido - Mostrando estado de error");
                  Debug.WriteLine($"💬 [ApprenticeDashboardVM] Mensaje diagnóstico: {DiagnosticMessage}");
                
       // Mostrar datos en modo offline/error
         Dashboard = new ApprenticeDashboardDto
             {
        HasRequest = false,
         RequestState = "Error de conexión",
    ShowInstructor = false
               };
          return;
         }
         
        Debug.WriteLine($"✅ [ApprenticeDashboardVM] Diagnóstico exitoso - Continuando con petición principal");
    
         // Pequeño delay para que la animación sea visible
        Debug.WriteLine($"⏱️ [ApprenticeDashboardVM] Delay de 500ms para animación...");
            await Task.Delay(500);
        
        Debug.WriteLine($"🚀 [ApprenticeDashboardVM] Llamando al servicio de asignación...");
           var result = await _assignService.GetApprenticeDashboardAsync(apprenticeId);
        
      Debug.WriteLine($"");
         Debug.WriteLine($"📥 [ApprenticeDashboardVM] ===== RESPUESTA DEL SERVICIO =====");
           Debug.WriteLine($"📊 [ApprenticeDashboardVM] Result != null: {result != null}");
    
          if (result != null)
           {
         Debug.WriteLine($"📋 [ApprenticeDashboardVM] HasRequest: {result.HasRequest}");
           Debug.WriteLine($"🎯 [ApprenticeDashboardVM] RequestState: {result.RequestState ?? "NULL"}");
        Debug.WriteLine($"👨‍🏫 [ApprenticeDashboardVM] ShowInstructor: {result.ShowInstructor}");
                
         if (result.Request != null)
      {
        Debug.WriteLine($"🏢 [ApprenticeDashboardVM] Enterprise: {result.Request.EnterpriseName ?? "NULL"}");
           Debug.WriteLine($"👨‍💼 [ApprenticeDashboardVM] Boss: {result.Request.BossName ?? "NULL"}");
        Debug.WriteLine($"📋 [ApprenticeDashboardVM] Modality: {result.Request.Modality ?? "NULL"}");
            Debug.WriteLine($"🎨 [ApprenticeDashboardVM] StateColor: {result.Request.StateColor ?? "NULL"}");
            Debug.WriteLine($"📁 [ApprenticeDashboardVM] HasPdf: {result.Request.HasPdf}");
         }
      else
      {
   Debug.WriteLine($"⚠️ [ApprenticeDashboardVM] Request es NULL");
         }
          
        if (result.Instructor != null)
    {
        Debug.WriteLine($"👨‍🏫 [ApprenticeDashboardVM] Instructor ID: {result.Instructor.Id}");
    Debug.WriteLine($"👤 [ApprenticeDashboardVM] Instructor Name: {result.Instructor.FirstName} {result.Instructor.FirstLastName}");
  Debug.WriteLine($"📧 [ApprenticeDashboardVM] Instructor Email: {result.Instructor.Email ?? "NULL"}");
        Debug.WriteLine($"🎓 [ApprenticeDashboardVM] Knowledge Area: {result.Instructor.KnowledgeArea ?? "NULL"}");
   Debug.WriteLine($"🤝 [ApprenticeDashboardVM] ShowContact: {result.Instructor.ShowContact}");
      }
    else
       {
   Debug.WriteLine($"⚠️ [ApprenticeDashboardVM] Instructor es NULL");
       }
 }
    else
         {
  Debug.WriteLine($"❌ [ApprenticeDashboardVM] Result es NULL");
       }
    
            Debug.WriteLine($"🔄 [ApprenticeDashboardVM] Asignando resultado al Dashboard...");
         Dashboard = result;
               ApprenticeId = apprenticeId;

       if (Dashboard != null)
    {
          ApprenticesCount = 125000000;
      UnassignedRequestsCount = 20;
          AssignedRequestsCount = 30;
      Debug.WriteLine($"✅ [ApprenticeDashboardVM] Dashboard asignado y contadores configurados");
        Debug.WriteLine($"📊 [ApprenticeDashboardVM] Valores finales del ViewModel:");
    Debug.WriteLine($"   📋 HasRequest: {HasRequest}");
          Debug.WriteLine($"   🎯 RequestStateLabel: {RequestStateLabel}");
     Debug.WriteLine($"   🎨 RequestStateColor: {RequestStateColor}");
      Debug.WriteLine($"   💬 RequestStateDescription: {RequestStateDescription}");
    Debug.WriteLine($"   🔘 RequestActionText: {RequestActionText}");
     Debug.WriteLine($"   👨‍🏫 InstructorFullName: {InstructorFullName}");
       Debug.WriteLine($"   👀 ShowRequestDetail: {ShowRequestDetail}");
   Debug.WriteLine($"   🚫 ShowEmptyRequestState: {ShowEmptyRequestState}");
          }
     else
          {
   Debug.WriteLine($"⚠️ [ApprenticeDashboardVM] Dashboard sigue siendo NULL después de asignación");
     }
         }
    catch (Exception ex)
   {
            Debug.WriteLine($"");
  Debug.WriteLine($"❌ [ApprenticeDashboardVM] ===== ERROR EN CARGA =====");
     Debug.WriteLine($"🚨 [ApprenticeDashboardVM] Exception Type: {ex.GetType().Name}");
      Debug.WriteLine($"💬 [ApprenticeDashboardVM] Error Message: {ex.Message}");
       Debug.WriteLine($"📍 [ApprenticeDashboardVM] StackTrace: {ex.StackTrace}");
   
    if (ex.InnerException != null)
       {
        Debug.WriteLine($"🔗 [ApprenticeDashboardVM] Inner Exception Type: {ex.InnerException.GetType().Name}");
   Debug.WriteLine($"💬 [ApprenticeDashboardVM] Inner Message: {ex.InnerException.Message}");
       Debug.WriteLine($"📍 [ApprenticeDashboardVM] Inner StackTrace: {ex.InnerException.StackTrace}");
    }
          
      // Mostrar mensaje de diagnóstico específico
      DiagnosticMessage = $"❌ Error: {ex.Message}";
       ShowDiagnosticMessage = true;
       Debug.WriteLine($"💬 [ApprenticeDashboardVM] Mensaje diagnóstico mostrado: {DiagnosticMessage}");
              
      // Establecer un dashboard vacío para mostrar el estado "sin solicitudes"
           Dashboard = new ApprenticeDashboardDto
 {
        HasRequest = false,
    RequestState = "Error al cargar datos",
   ShowInstructor = false
      };
       
    Debug.WriteLine($"🔄 [ApprenticeDashboardVM] Dashboard configurado en estado de error");
       }
        finally
     {
     IsLoading = false;
     Debug.WriteLine($"🏁 [ApprenticeDashboardVM] ===== CARGA FINALIZADA =====");
           Debug.WriteLine($"🔄 [ApprenticeDashboardVM] IsLoading: {IsLoading}");
     Debug.WriteLine($"💬 [ApprenticeDashboardVM] ShowDiagnosticMessage: {ShowDiagnosticMessage}");
      Debug.WriteLine($"📊 [ApprenticeDashboardVM] Dashboard != null: {Dashboard != null}");
         Debug.WriteLine($"");
     }
        }

        private async Task DiagnoseConnectionAsync()
        {
     try
     {
     Debug.WriteLine($"");
   Debug.WriteLine($"🔍 [ApprenticeDashboardVM] ===== DIAGNÓSTICO MANUAL =====");
      Debug.WriteLine("[ApprenticeDashboardVM] Iniciando diagnóstico manual...");
        DiagnosticMessage = "🔍 Diagnosticando conexión...";
ShowDiagnosticMessage = true;
      
 Debug.WriteLine($"🌐 [ApprenticeDashboardVM] URL Base: {Endpoints.API_BASE_URL}");
       Debug.WriteLine($"🔗 [ApprenticeDashboardVM] Endpoint de prueba: security/document-types/");
    
        var (isConnected, message) = await _apiService.DiagnoseConnectionAsync();
       
       Debug.WriteLine($"📊 [ApprenticeDashboardVM] Resultado diagnóstico manual:");
    Debug.WriteLine($"   ✅ Conectado: {isConnected}");
         Debug.WriteLine($"   💬 Mensaje: {message}");
   
 DiagnosticMessage = isConnected ? $"✅ {message}" : $"❌ {message}";
     
  // Información adicional de diagnóstico
#if ANDROID
              var platformInfo = "📱 Plataforma: Android";
#elif IOS
 var platformInfo = "📱 Plataforma: iOS";
#else
  var platformInfo = "📱 Plataforma: Windows";
#endif
        var networkInfo = $"\n🌐 URL: {Endpoints.API_BASE_URL}";
         DiagnosticMessage += $"\n{platformInfo}" + networkInfo;
          
        Debug.WriteLine($"💬 [ApprenticeDashboardVM] Mensaje final diagnóstico: {DiagnosticMessage}");
   Debug.WriteLine($"🏁 [ApprenticeDashboardVM] ===== FIN DIAGNÓSTICO MANUAL =====");
       Debug.WriteLine($"");
   }
      catch (Exception ex)
      {
      Debug.WriteLine($"❌ [ApprenticeDashboardVM] Error en diagnóstico manual: {ex.Message}");
     DiagnosticMessage = $"❌ Error en diagnóstico: {ex.Message}";
    }
    }

        private void LoadApprenticeMetadata()
        {
            try
            {
                var userDataRaw = Preferences.Get("user_data", string.Empty);
                if (!string.IsNullOrWhiteSpace(userDataRaw))
                {
                    using var document = JsonDocument.Parse(userDataRaw);
                    if (document.RootElement.TryGetProperty("firstName", out var firstName) &&
                        !string.IsNullOrWhiteSpace(firstName.GetString()))
                    {
                        ApprenticeName = firstName.GetString()!;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ApprenticeDashboardVM] Error parsing user_data: {ex}");
            }

            var fallback = Preferences.Get("UserEmail", string.Empty);
            ApprenticeName = string.IsNullOrWhiteSpace(fallback) ? "Aprendiz" : fallback;
        }

        private void RaiseDashboardDerivedProperties()
        {
            OnPropertyChanged(nameof(HasRequest));
            OnPropertyChanged(nameof(ShowRequestDetail));
            OnPropertyChanged(nameof(ShowEmptyRequestState));
            OnPropertyChanged(nameof(RequestStateLabel));
            OnPropertyChanged(nameof(RequestStateColor));
            OnPropertyChanged(nameof(RequestStateDescription));
            OnPropertyChanged(nameof(RequestActionText));
            OnPropertyChanged(nameof(EnterpriseName));
            OnPropertyChanged(nameof(BossName));
            OnPropertyChanged(nameof(ModalityDisplay));
            OnPropertyChanged(nameof(RequestDateDisplay));
            OnPropertyChanged(nameof(StartDateDisplay));
            OnPropertyChanged(nameof(EndDateDisplay));
            OnPropertyChanged(nameof(CityDisplay));
            OnPropertyChanged(nameof(ShowPdfButton));
            OnPropertyChanged(nameof(RequestPdfUrl));
            OnPropertyChanged(nameof(InstructorFullName));
            OnPropertyChanged(nameof(HasInstructor));
            OnPropertyChanged(nameof(ShowInstructorCard));
            OnPropertyChanged(nameof(ShowInstructorContact));
            OnPropertyChanged(nameof(InstructorKnowledgeArea));
            OnPropertyChanged(nameof(InstructorContactEmail));
            OnPropertyChanged(nameof(InstructorContactPhone));
            OnPropertyChanged(nameof(InstructorAssignedAtDisplay));
            OnPropertyChanged(nameof(InstructorTagText));
            OnPropertyChanged(nameof(InstructorTagBackground));
        }

        private static string FormatDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "Sin definir";
            }

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
            {
                return parsed.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return value;
        }

        private async Task OpenPdfAsync()
        {
            if (!ShowPdfButton)
            {
                return;
            }

            try
            {
                if (Uri.TryCreate(RequestPdfUrl, UriKind.Absolute, out var uri))
                {
                    await Launcher.OpenAsync(uri);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ApprenticeDashboardVM] Error al abrir PDF: {ex}");
            }
        }
    }
}
