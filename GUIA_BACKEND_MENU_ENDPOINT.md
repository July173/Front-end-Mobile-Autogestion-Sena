# 📡 Guía del Backend: Endpoint de Menú Dinámico

## 🎯 Endpoint Requerido

```
GET /security/rol-form-permissions/{roleId}/get-menu
```

### Parámetros

- `roleId` (int): ID del rol del usuario que solicita el menú

### Headers

```
Authorization: Bearer {token}
Content-Type: application/json
```

---

## 📦 Estructura de Response

### Response DTO

```json
{
  "roleId": 1,
  "roleName": "Administrador",
  "menuItems": [
    {
      "id": 1,
      "name": "Seguridad",
      "icon": "\\uf59d",
      "route": "security",
      "parentId": null,
      "order": 1,
      "isExpanded": false,
      "subMenus": [
        {
          "id": 2,
          "name": "Asignar seguimiento",
          "icon": null,
          "route": "security/assign",
          "parentId": 1,
          "order": 1,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 3,
          "name": "Reasignar",
          "icon": null,
          "route": "security/reassign",
          "parentId": 1,
          "order": 2,
          "isExpanded": false,
          "subMenus": []
        }
      ]
    }
  ]
}
```

### Campos Explicados

| Campo | Tipo | Required | Descripción |
|-------|------|----------|-------------|
| `roleId` | int | ✅ | ID del rol |
| `roleName` | string | ✅ | Nombre del rol (ej: "Administrador") |
| `menuItems` | array | ✅ | Lista de items del menú principal |
| `id` | int | ✅ | ID único del item |
| `name` | string | ✅ | Nombre visible del menú |
| `icon` | string | ❌ | Código Unicode del Bootstrap Icon |
| `route` | string | ✅ | Ruta de navegación |
| `parentId` | int? | ❌ | ID del item padre (null si es raíz) |
| `order` | int | ✅ | Orden de aparición (1, 2, 3...) |
| `isExpanded` | bool | ✅ | Si está expandido por defecto |
| `subMenus` | array | ✅ | Lista de submenús (puede estar vacío) |

---

## 🎨 Iconos Bootstrap

### ⚠️ IMPORTANTE: Usar Bootstrap Icons (NO archivos PNG)

El frontend usa la fuente **Bootstrap Icons** para todos los iconos del menú.

### Códigos Unicode para los Iconos Principales

```csharp
// Navegación
Inicio           → "\uf425"  // bi-house-fill
Dashboard        → "\uf3eb"  // bi-grid-fill

// Seguridad
Seguridad        → "\uf59d"  // bi-shield-fill
Permisos         → "\uf3ca"  // bi-fingerprint
Roles            → "\uf4d3"  // bi-person-badge-fill
Usuarios         → "\uf4cf"  // bi-people-fill

// Formularios y Documentos
Formularios      → "\uf2a6"  // bi-clipboard-check-fill
Documentos       → "\uf3c2"  // bi-file-text-fill
Reportes         → "\uf212"  // bi-bar-chart-fill

// Asignaciones
Asignar          → "\uf4e8"  // bi-person-workspace
Seguimiento      → "\uf358"  // bi-eye-fill
Evaluar          → "\uf26a"  // bi-check-circle-fill

// Configuración
Configuración    → "\uf3e2"  // bi-gear-fill
Módulos          → "\uf3d9"  // bi-folder-fill
```

### Referencia Completa

Ver archivo **GUIA_BOOTSTRAP_ICONS.md** en el frontend para la lista completa de iconos disponibles.

### Búsqueda de Iconos

1. Ir a: https://icons.getbootstrap.com/
2. Buscar el icono deseado
3. Copiar el código (ej: `f425`)
4. Agregar prefijo `\u`: `\uf425`
5. Usar en JSON con escape doble: `"\\uf425"`

---

## 🔐 Lógica del Backend

### Flujo Recomendado

```
1. Validar token de autenticación
   ↓
2. Obtener roleId del parámetro
   ↓
3. Verificar que el usuario tenga ese rol
   ↓
4. Consultar permisos del rol en BD
   ↓
5. Filtrar formularios/módulos permitidos
   ↓
6. Construir estructura jerárquica del menú
   ↓
7. Asignar iconos según tipo de módulo
   ↓
8. Ordenar items por campo 'order'
   ↓
9. Retornar MenuResponseDto
```

### Ejemplo de Consulta SQL (Conceptual)

```sql
-- Obtener permisos del rol
SELECT 
    f.id,
    f.name,
    f.route,
    m.name as module_name,
    rfp.can_view,
    rfp.can_create,
    rfp.can_update,
    rfp.can_delete
FROM rol_form_permissions rfp
INNER JOIN forms f ON rfp.form_id = f.id
INNER JOIN modules m ON f.module_id = m.id
WHERE rfp.rol_id = @roleId
  AND rfp.can_view = true
ORDER BY m.order, f.order;
```

### Ejemplo de Código Backend (C#)

```csharp
[HttpGet("security/rol-form-permissions/{roleId}/get-menu")]
public async Task<ActionResult<MenuResponseDto>> GetMenuByRole(int roleId)
{
    // 1. Validar autenticación
    var user = await GetCurrentUser();
    if (user == null)
        return Unauthorized();

    // 2. Obtener rol
    var role = await _context.Roles
        .Include(r => r.RolFormPermissions)
        .ThenInclude(rfp => rfp.Form)
        .ThenInclude(f => f.Module)
        .FirstOrDefaultAsync(r => r.Id == roleId);

    if (role == null)
        return NotFound();

    // 3. Construir menú
    var menuItems = new List<MenuDto>();
    
    // Agrupar por módulos
    var modules = role.RolFormPermissions
        .Where(rfp => rfp.CanView)
        .Select(rfp => rfp.Form.Module)
        .Distinct()
        .OrderBy(m => m.Order);

    foreach (var module in modules)
    {
        var moduleMenu = new MenuDto
        {
            Id = module.Id,
            Name = module.Name,
            Icon = GetIconForModule(module.Name), // ← Asignar icono
            Route = module.Route,
            ParentId = null,
            Order = module.Order,
            IsExpanded = false,
            SubMenus = new List<MenuDto>()
        };

        // Agregar formularios como submenús
        var forms = role.RolFormPermissions
            .Where(rfp => rfp.CanView && rfp.Form.ModuleId == module.Id)
            .Select(rfp => rfp.Form)
            .OrderBy(f => f.Order);

        foreach (var form in forms)
        {
            moduleMenu.SubMenus.Add(new MenuDto
            {
                Id = form.Id,
                Name = form.Name,
                Icon = null, // Submenús sin icono
                Route = $"{module.Route}/{form.Route}",
                ParentId = module.Id,
                Order = form.Order,
                IsExpanded = false,
                SubMenus = new List<MenuDto>()
            });
        }

        menuItems.Add(moduleMenu);
    }

    // 4. Retornar response
    return Ok(new MenuResponseDto
    {
        RoleId = role.Id,
        RoleName = role.Name,
        MenuItems = menuItems
    });
}

// Método helper para asignar iconos
private string GetIconForModule(string moduleName)
{
    return moduleName.ToLower() switch
    {
        "seguridad" => "\\uf59d",        // bi-shield-fill
        "inicio" => "\\uf425",           // bi-house-fill
        "usuarios" => "\\uf4cf",         // bi-people-fill
        "formularios" => "\\uf2a6",      // bi-clipboard-check-fill
        "reportes" => "\\uf212",         // bi-bar-chart-fill
        "configuración" => "\\uf3e2",    // bi-gear-fill
        _ => "\\uf3eb"                   // bi-grid-fill (default)
    };
}
```

---

## 🧪 Ejemplos de Response

### Ejemplo 1: Rol Administrador (Acceso Completo)

```json
{
  "roleId": 1,
  "roleName": "Administrador",
  "menuItems": [
    {
      "id": 1,
      "name": "Seguridad",
      "icon": "\\uf59d",
      "route": "security",
      "parentId": null,
      "order": 1,
      "isExpanded": false,
      "subMenus": [
        {
          "id": 11,
          "name": "Asignar seguimiento",
          "icon": null,
          "route": "security/assign",
          "parentId": 1,
          "order": 1,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 12,
          "name": "Reasignar",
          "icon": null,
          "route": "security/reassign",
          "parentId": 1,
          "order": 2,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 13,
          "name": "Seguimiento de visitas",
          "icon": null,
          "route": "security/follow-up",
          "parentId": 1,
          "order": 3,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 14,
          "name": "Historial",
          "icon": null,
          "route": "security/history",
          "parentId": 1,
          "order": 4,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 15,
          "name": "Evaluar visita",
          "icon": null,
          "route": "security/evaluate",
          "parentId": 1,
          "order": 5,
          "isExpanded": false,
          "subMenus": []
        }
      ]
    },
    {
      "id": 2,
      "name": "Usuarios",
      "icon": "\\uf4cf",
      "route": "users",
      "parentId": null,
      "order": 2,
      "isExpanded": false,
      "subMenus": [
        {
          "id": 21,
          "name": "Gestionar usuarios",
          "icon": null,
          "route": "users/manage",
          "parentId": 2,
          "order": 1,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 22,
          "name": "Roles y permisos",
          "icon": null,
          "route": "users/roles",
          "parentId": 2,
          "order": 2,
          "isExpanded": false,
          "subMenus": []
        }
      ]
    }
  ]
}
```

### Ejemplo 2: Rol Instructor (Acceso Limitado)

```json
{
  "roleId": 4,
  "roleName": "Instructor",
  "menuItems": [
    {
      "id": 1,
      "name": "Seguimiento",
      "icon": "\\uf358",
      "route": "follow-up",
      "parentId": null,
      "order": 1,
      "isExpanded": false,
      "subMenus": [
        {
          "id": 11,
          "name": "Mis asignaciones",
          "icon": null,
          "route": "follow-up/my-assignments",
          "parentId": 1,
          "order": 1,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 12,
          "name": "Registrar visita",
          "icon": null,
          "route": "follow-up/register",
          "parentId": 1,
          "order": 2,
          "isExpanded": false,
          "subMenus": []
        }
      ]
    }
  ]
}
```

### Ejemplo 3: Rol Aprendiz (Solo Lectura)

```json
{
  "roleId": 5,
  "roleName": "Aprendiz",
  "menuItems": [
    {
      "id": 1,
      "name": "Mis Datos",
      "icon": "\\uf4da",
      "route": "profile",
      "parentId": null,
      "order": 1,
      "isExpanded": false,
      "subMenus": [
        {
          "id": 11,
          "name": "Ver perfil",
          "icon": null,
          "route": "profile/view",
          "parentId": 1,
          "order": 1,
          "isExpanded": false,
          "subMenus": []
        },
        {
          "id": 12,
          "name": "Historial de visitas",
          "icon": null,
          "route": "profile/visits",
          "parentId": 1,
          "order": 2,
          "isExpanded": false,
          "subMenus": []
        }
      ]
    }
  ]
}
```

---

## ✅ Validaciones Requeridas

### 1. Validación de Autenticación
```csharp
if (!User.Identity.IsAuthenticated)
    return Unauthorized();
```

### 2. Validación de Rol
```csharp
// El usuario debe tener el rol solicitado
var userRoles = await _userManager.GetRolesAsync(currentUser);
if (!userRoles.Any(r => r.Id == roleId))
    return Forbidden();
```

### 3. Validación de Permisos
```csharp
// Solo mostrar formularios con can_view = true
var visibleForms = permissions.Where(p => p.CanView);
```

### 4. Validación de Datos
```csharp
// Asegurar que todos los campos requeridos estén presentes
if (string.IsNullOrEmpty(menuItem.Name))
    throw new ValidationException("Name is required");

if (string.IsNullOrEmpty(menuItem.Route))
    throw new ValidationException("Route is required");
```

---

## 🔍 Testing del Endpoint

### Herramientas Recomendadas
- **Postman**
- **Swagger UI**
- **curl**

### Ejemplo con curl

```bash
curl -X GET "https://api.tudominio.com/security/rol-form-permissions/1/get-menu" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -H "Content-Type: application/json"
```

### Ejemplo con Postman

```
GET https://api.tudominio.com/security/rol-form-permissions/1/get-menu

Headers:
  Authorization: Bearer {token}
  Content-Type: application/json
```

### Response Esperado (Status 200)

```json
{
  "roleId": 1,
  "roleName": "Administrador",
  "menuItems": [...]
}
```

### Errores Posibles

| Status | Descripción | Response |
|--------|-------------|----------|
| 401 | No autenticado | `{ "error": "Unauthorized" }` |
| 403 | Sin permiso | `{ "error": "Forbidden" }` |
| 404 | Rol no encontrado | `{ "error": "Role not found" }` |
| 500 | Error servidor | `{ "error": "Internal server error" }` |

---

## 📊 Diagrama de Flujo

```
┌─────────────────┐
│   Frontend      │
│  (MAUI App)     │
└────────┬────────┘
         │
         │ GET /security/rol-form-permissions/{roleId}/get-menu
         │ Authorization: Bearer {token}
         ▼
┌─────────────────┐
│   Backend API   │
│                 │
│ 1. Validar token│
│ 2. Buscar rol   │
│ 3. Filtrar perms│
│ 4. Construir DTO│
│ 5. Asignar icons│
└────────┬────────┘
         │
         │ MenuResponseDto (JSON)
         ▼
┌─────────────────┐
│   Frontend      │
│                 │
│ 1. Deserializar │
│ 2. Crear ViewM. │
│ 3. Renderizar   │
└─────────────────┘
```

---

## 🚀 Checklist de Implementación

- [ ] Crear endpoint en el controller
- [ ] Implementar DTOs (MenuDto, MenuResponseDto)
- [ ] Crear consulta a BD para permisos
- [ ] Implementar lógica de filtrado por rol
- [ ] Asignar iconos Bootstrap según módulo
- [ ] Ordenar items por campo 'order'
- [ ] Agregar validaciones de autenticación
- [ ] Agregar validaciones de autorización
- [ ] Implementar manejo de errores
- [ ] Probar con diferentes roles
- [ ] Documentar en Swagger
- [ ] Agregar logs para debugging

---

## 📞 Soporte

Si tienes dudas sobre la implementación:

1. Revisar **GUIA_BOOTSTRAP_ICONS.md** para iconos
2. Revisar **README_DYNAMIC_MENU.md** para estructura frontend
3. Consultar ejemplos de response en este documento
4. Verificar que el formato JSON coincida con los DTOs

---

**Autor**: Equipo Frontend SENA  
**Fecha**: Noviembre 2025  
**Versión**: 1.0
