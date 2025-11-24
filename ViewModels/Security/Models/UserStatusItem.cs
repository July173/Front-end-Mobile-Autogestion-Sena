using Microsoft.Maui.Graphics;

namespace AutogestionSenaMaui.ViewModels.Security.Models;

public class UserStatusItem
{
    public string Title { get; set; } = string.Empty;
    public int Count { get; set; }
    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public Color AccentColor { get; set; } = Colors.Black;
    public Color BadgeBackgroundColor { get; set; } = Colors.Transparent;
    public Color BadgeTextColor { get; set; } = Colors.Black;
}
