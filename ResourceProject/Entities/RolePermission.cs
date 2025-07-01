using ResourceProject.Entities.JoinEntities;

namespace ResourceProject.Entities;

public class RolePermission
{
    public Guid Id { get; set; }
    public string PermissionName { get; set; }

    // Navigation property to Roles (many-to-many relationship)
    public ICollection<RolePermissionRole> RolePermissionRoles { get; set; }
}
