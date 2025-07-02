using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Queries;

public class RolePermissionQuery
{
    private readonly IRolePermissionService _rolePermissionService;

    public RolePermissionQuery(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    // Query to get all permissions for a specific role
    public async Task<IEnumerable<RolePermissionDto>> GetPermissionsForRoleAsync(Guid roleId)
    {
        return await _rolePermissionService.GetPermissionsForRoleAsync(roleId);
    }

    // Query to get a specific permission for a role
    public async Task<RolePermissionDto> GetPermissionForRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _rolePermissionService.GetPermissionForRoleAsync(roleId, permissionId);
    }
}
