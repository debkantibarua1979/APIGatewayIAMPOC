namespace ResourceProject.DTOs;

public class AssignRolePermissionDTO
{
    public Guid RoleId { get; set; }
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}