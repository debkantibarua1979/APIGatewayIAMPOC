namespace ResourceProject.Entities.JoinEntities;

public class RolePermissionRole
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public Guid PermissionId { get; set; }
    public RolePermission RolePermission { get; set; }
}