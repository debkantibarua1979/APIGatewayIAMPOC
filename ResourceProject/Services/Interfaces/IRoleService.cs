using ResourceProject.DTOs;

namespace ResourceProject.Services.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetRolesAsync();
    Task<RoleDto> GetRoleByIdAsync(Guid roleId);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
    Task<RoleDto> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto);
    Task<bool> DeleteRoleAsync(Guid roleId);
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);
}