namespace AutogestionSenaMaui.Api.Dtos;

/// <summary>
/// DTO para el menú dinámico basado en permisos del rol
/// </summary>
public class MenuDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Route { get; set; }
    public int? ParentId { get; set; }
    public int Order { get; set; }
    public bool IsExpanded { get; set; }
    public List<MenuDto> SubMenus { get; set; } = new();
}

/// <summary>
/// DTO para la respuesta del endpoint de menú
/// </summary>
public class MenuResponseDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<MenuDto> MenuItems { get; set; } = new();
}
