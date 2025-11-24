using System.Collections.ObjectModel;
using AutogestionSenaMaui.ViewModels.Security.Models;

namespace AutogestionSenaMaui.ViewModels.Security
{
    public class SecuritySummaryViewModel
    {
        public ObservableCollection<PermissionMatrixItem> PermissionMatrix { get; }
        public ObservableCollection<RoleDistributionItem> RoleDistribution { get; }

        public SecuritySummaryViewModel()
        {
            PermissionMatrix = new ObservableCollection<PermissionMatrixItem>
            {
                new PermissionMatrixItem { Role = "Administrador", FormName = "Crear Usuario", CanView = true, CanEdit = true, CanDelete = false, CanToggle = false },
                new PermissionMatrixItem { Role = "Administrador", FormName = "Editar Usuario", CanView = false, CanEdit = false, CanDelete = false, CanToggle = false },
                new PermissionMatrixItem { Role = "Administrador", FormName = "Reporte Ventas", CanView = false, CanEdit = false, CanDelete = false, CanToggle = false },
                new PermissionMatrixItem { Role = "Usuario", FormName = "Crear Usuario", CanView = true, CanEdit = false, CanDelete = false, CanToggle = false },
                new PermissionMatrixItem { Role = "Usuario", FormName = "Editar Usuario", CanView = false, CanEdit = false, CanDelete = false, CanToggle = false },
                new PermissionMatrixItem { Role = "Usuario", FormName = "Reporte Ventas", CanView = false, CanEdit = false, CanDelete = false, CanToggle = false }
            };

            RoleDistribution = new ObservableCollection<RoleDistributionItem>
            {
                new RoleDistributionItem { Role = "Administrador", Description = "Acceso completo al sistema", Count = 3, CountLabel = "Administradores" },
                new RoleDistributionItem { Role = "Usuarios", Description = "Usuarios del sistema", Count = 22, CountLabel = "Usuarios" },
                new RoleDistributionItem { Role = "Aprendices", Description = "Aprendices del sistema", Count = 3321, CountLabel = "Aprendices" },
                new RoleDistributionItem { Role = "Instructores", Description = "Instructores del sistema", Count = 33, CountLabel = "Instructores" },
                new RoleDistributionItem { Role = "Coordinadores", Description = "Coordinadores del sistema", Count = 1, CountLabel = "Coordinadores" }
            };
        }
    }
}
