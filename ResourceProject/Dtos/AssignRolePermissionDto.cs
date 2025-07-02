namespace ResourceProject.DTOs;

public class AssignRolePermissionDto
{
    public Guid RoleId { get; set; }
    public List<Guid> PermissionIds { get; set; } = new List<Guid>();
}