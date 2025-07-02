using ResourceProject.Entities;

namespace ResourceProject.Repositories.Interfaces;

public interface IRolePermissionRepository
{
    Task<List<RolePermission>> GetAllAsync();
    Task<RolePermission?> GetByIdAsync(Guid id);
    Task<RolePermission> CreateAsync(RolePermission rolePermission);
    Task<RolePermission> UpdateAsync(RolePermission rolePermission);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds);
    Task<bool> RemovePermissionsFromRoleAsync(Guid roleId, List<Guid> permissionIds);
}