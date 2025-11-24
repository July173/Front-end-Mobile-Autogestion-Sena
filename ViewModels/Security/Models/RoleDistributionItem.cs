namespace AutogestionSenaMaui.ViewModels.Security.Models
{
    public class RoleDistributionItem
    {
        public string Role { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Count { get; set; }
        public string CountLabel { get; set; } = string.Empty;
    }
}
