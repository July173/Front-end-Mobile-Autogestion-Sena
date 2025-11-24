using System.Collections.ObjectModel;
using System.Windows.Input;
using AutogestionSenaMaui.ViewModels.Security.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace AutogestionSenaMaui.ViewModels.Security;

public class SecurityUsersViewModel
{
    public SecurityUsersViewModel()
    {
        PanelTitle = "Gestion De Usuarios-Sena";
        SearchPlaceholder = "Buscar por número de documento";
        SelectedUserFilter = "Todos los usuarios";
        SelectedStateFilter = "Todos los estados";

        RegisterUserCommand = new Command(() => { /* TODO: Wire navigation to registration */ });

        StatusCards = new ObservableCollection<UserStatusItem>
        {
            new()
            {
                Title = "Usuarios habilitados",
                Count = 2,
                BackgroundColor = Color.FromArgb("#ECFDF3"),
                AccentColor = Color.FromArgb("#16A34A"),
                BadgeBackgroundColor = Color.FromArgb("#BBF7D0"),
                BadgeTextColor = Color.FromArgb("#16A34A")
            },
            new()
            {
                Title = "Usuarios Registrados",
                Count = 1,
                BackgroundColor = Color.FromArgb("#FEF9C3"),
                AccentColor = Color.FromArgb("#A16207"),
                BadgeBackgroundColor = Color.FromArgb("#FEF08A"),
                BadgeTextColor = Color.FromArgb("#A16207")
            },
            new()
            {
                Title = "Usuarios Inhabilitados",
                Count = 5,
                BackgroundColor = Color.FromArgb("#FEE2E2"),
                AccentColor = Color.FromArgb("#B91C1C"),
                BadgeBackgroundColor = Color.FromArgb("#FECACA"),
                BadgeTextColor = Color.FromArgb("#B91C1C")
            }
        };
    }

    public string PanelTitle { get; }
    public string SearchPlaceholder { get; }
    public string SelectedUserFilter { get; }
    public string SelectedStateFilter { get; }

    public string SearchText { get; set; } = string.Empty;

    public ObservableCollection<UserStatusItem> StatusCards { get; }

    public ICommand RegisterUserCommand { get; }
}
