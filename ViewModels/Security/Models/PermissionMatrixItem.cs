namespace AutogestionSenaMaui.ViewModels.Security.Models
{
    public class PermissionMatrixItem
    {
        public string Role { get; set; } = string.Empty;
        public string FormName { get; set; } = string.Empty;
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanToggle { get; set; }
    }
}
