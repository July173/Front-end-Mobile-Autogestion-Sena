using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using AutogestionSena.MAUI.Api.Services;
using AutogestionSena.MAUI.Api.Dtos;

namespace AutogestionSenaMaui.ViewModels
{
    public class ApprenticeDashboardViewModel : BindableObject
    {
        private readonly AssignationService _assignService;
        private bool _isLoading;
        private ApprenticeDashboardDto? _dashboard;
        private int _apprenticesCount;
        private int _unassignedRequestsCount;
        private int _assignedRequestsCount;
    public int ApprenticesCount { get => _apprenticesCount; set { _apprenticesCount = value; OnPropertyChanged(); } }
    public int UnassignedRequestsCount { get => _unassignedRequestsCount; set { _unassignedRequestsCount = value; OnPropertyChanged(); } }
    public int AssignedRequestsCount { get => _assignedRequestsCount; set { _assignedRequestsCount = value; OnPropertyChanged(); } }

        public ApprenticeDashboardViewModel()
        {
            _assignService = new AssignationService();
            RefreshCommand = new Command(async () => await LoadAsync(ApprenticeId));
        }

        public int ApprenticeId { get; set; }

        public ApprenticeDashboardDto? Dashboard
        {
            get => _dashboard;
            set { _dashboard = value; OnPropertyChanged(); }
        }

        public bool HasRequest => Dashboard?.HasRequest ?? false;

        public string InstructorFullName
        {
            get
            {
                var i = Dashboard?.Instructor;
                if (i == null) return string.Empty;
                return $"{i.FirstName} {i.SecondName ?? string.Empty} {i.FirstLastName} {i.SecondLastName ?? string.Empty}".Trim();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }

        public async Task LoadAsync(int apprenticeId)
        {
            try
            {
                IsLoading = true;
                var result = await _assignService.GetApprenticeDashboardAsync(apprenticeId);
                Dashboard = result;
                ApprenticeId = apprenticeId;
                // Set DashboardCardsData sample counts; in future, call service endpoints
                if (Dashboard != null)
                {
                    ApprenticesCount = 125000000;
                    UnassignedRequestsCount = 20;
                    AssignedRequestsCount = 30;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApprenticeDashboardVM] Error loading apprentice dashboard: {ex}");
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
