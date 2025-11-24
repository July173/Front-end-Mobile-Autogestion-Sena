using System.Collections.ObjectModel;
using System.Windows.Input;
using AutogestionSenaMaui.ViewModels.Security.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace AutogestionSenaMaui.ViewModels.Security;

public class SecurityRolesViewModel
{
    public SecurityRolesViewModel()
    {
        PanelTitle = "Gestion De Roles-Sena";
        SearchPlaceholder = "Buscar por rol";
        SelectedRoleFilter = "Todos los roles";

        RegisterRoleCommand = new Command(() => { /* Hook into real navigation later */ });
        EditRoleCommand = new Command<RoleCardItem?>(_ => { /* Hook into editor */ });
        DisableRoleCommand = new Command<RoleCardItem?>(_ => { /* Hook into backend */ });

        Roles = new ObservableCollection<RoleCardItem>
        {
            new()
            {
                Name = "Administrador",
                Description = "Acceso total al sistema",
                AssignedUsersText = "2 usuarios asignados",
                StatusLabel = "Activo",
                StatusBackground = Color.FromArgb("#D1FAE5"),
                StatusTextColor = Color.FromArgb("#166534")
            },
            new()
            {
                Name = "Instructor",
                Description = "Acceso parcial al sistema",
                AssignedUsersText = "2 usuarios asignados",
                StatusLabel = "Activo",
                StatusBackground = Color.FromArgb("#D1FAE5"),
                StatusTextColor = Color.FromArgb("#166534")
            },
            new()
            {
                Name = "Aprendiz",
                Description = "Acceso parcial al sistema",
                AssignedUsersText = "2 usuarios asignados",
                StatusLabel = "Activo",
                StatusBackground = Color.FromArgb("#D1FAE5"),
                StatusTextColor = Color.FromArgb("#166534")
            },
            new()
            {
                Name = "Coordinador",
                Description = "Acceso total al sistema",
                AssignedUsersText = "2 usuarios asignados",
                StatusLabel = "Activo",
                StatusBackground = Color.FromArgb("#D1FAE5"),
                StatusTextColor = Color.FromArgb("#166534")
            }
        };
    }

    public string PanelTitle { get; }
    public string SearchPlaceholder { get; }
    public string SelectedRoleFilter { get; }

    public string SearchText { get; set; } = string.Empty;

    public ObservableCollection<RoleCardItem> Roles { get; }

    public ICommand RegisterRoleCommand { get; }
    public ICommand EditRoleCommand { get; }
    public ICommand DisableRoleCommand { get; }
}
