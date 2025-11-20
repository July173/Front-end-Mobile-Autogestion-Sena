using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace AutogestionSenaMaui.ViewModels;

public class DashboardCardsViewModel : BindableObject
{
    private int _apprenticesCount;
    private int _unassignedRequestsCount;
    private int _assignedRequestsCount;
    private bool _isLoading;

    public int ApprenticesCount { get => _apprenticesCount; set { _apprenticesCount = value; OnPropertyChanged(); } }
    public int UnassignedRequestsCount { get => _unassignedRequestsCount; set { _unassignedRequestsCount = value; OnPropertyChanged(); } }
    public int AssignedRequestsCount { get => _assignedRequestsCount; set { _assignedRequestsCount = value; OnPropertyChanged(); } }
    public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); } }

    public DashboardCardsViewModel() { }

    public async Task LoadSampleAsync()
    {
        IsLoading = true;
        await Task.Delay(200); // Placeholder; later API calls here
        ApprenticesCount = 125000000;
        UnassignedRequestsCount = 20;
        AssignedRequestsCount = 30;
        IsLoading = false;
    }
}
