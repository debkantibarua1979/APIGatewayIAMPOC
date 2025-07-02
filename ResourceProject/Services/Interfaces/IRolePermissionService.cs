using ResourceProject.DTOs;

namespace ResourceProject.Services.Interfaces;

public interface IRolePermissionService
{
    Task<IEnumerable<RolePermissionDto>> GetPermissionsForRoleAsync(Guid roleId);
    Task<RolePermissionDto> GetPermissionForRoleAsync(Guid roleId, Guid permissionId);
    Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId);
    Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
    Task<IEnumerable<RolePermissionDto>> GetAllRolePermissions();
}