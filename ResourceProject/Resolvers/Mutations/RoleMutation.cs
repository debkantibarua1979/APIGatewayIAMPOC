using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Mutations;

public class RoleMutation
{
    private readonly IRoleService _roleService;

    public RoleMutation(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // Mutation to create a new role
    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        return await _roleService.CreateRoleAsync(createRoleDto);
    }

    // Mutation to update an existing role
    public async Task<RoleDto> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto)
    {
        return await _roleService.UpdateRoleAsync(roleId, updateRoleDto);
    }

    // Mutation to delete a role
    public async Task<bool> DeleteRoleAsync(Guid roleId)
    {
        return await _roleService.DeleteRoleAsync(roleId);
    }

    // Mutation to assign a permission to a role
    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
    {
        return await _roleService.AssignPermissionToRoleAsync(roleId, permissionId);
    }
}
