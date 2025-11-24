using Microsoft.Maui.Graphics;

namespace AutogestionSenaMaui.ViewModels.Security.Models;

public class RoleCardItem
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AssignedUsersText { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public Color StatusBackground { get; set; } = Colors.Transparent;
    public Color StatusTextColor { get; set; } = Colors.Black;
}
