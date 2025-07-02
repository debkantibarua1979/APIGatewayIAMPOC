namespace ResourceProject.DTOs;

public class RolePermissionDto
{
    public Guid RoleId { get; set; }              // The ID of the role
    public Guid PermissionId { get; set; }        // The ID of the permission
    public string PermissionName { get; set; }    // The name of the permission (e.g., "CanRead", "CanWrite")
}
