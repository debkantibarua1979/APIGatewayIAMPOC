using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Mutations;

public class RolePermissionMutation
{
    private readonly IRolePermissionService _rolePermissionService;

    public RolePermissionMutation(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    // Mutation to add a permission to a role
    public async Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _rolePermissionService.AddPermissionToRoleAsync(roleId, permissionId);
    }

    // Mutation to remove a permission from a role
    public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _rolePermissionService.RemovePermissionFromRoleAsync(roleId, permissionId);
    }
}
