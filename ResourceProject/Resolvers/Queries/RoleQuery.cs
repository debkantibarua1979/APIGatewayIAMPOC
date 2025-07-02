using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Queries;

public class RoleQuery
{
    private readonly IRoleService _roleService;

    public RoleQuery(IRoleService roleService)
    {
        _roleService = roleService;
    }

    // Query to get all roles
    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        return await _roleService.GetRolesAsync();
    }

    // Query to get a role by ID
    public async Task<RoleDto> GetRoleByIdAsync(Guid roleId)
    {
        return await _roleService.GetRoleByIdAsync(roleId);
    }
}
