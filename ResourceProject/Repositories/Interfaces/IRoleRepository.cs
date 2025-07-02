using ResourceProject.Entities;

namespace ResourceProject.Repositories.Interfaces;

public interface IRoleRepository
{
    // Create a new role
    Task<Role> CreateRoleAsync(Role role);

    // Update an existing role
    Task<Role> UpdateRoleAsync(Guid roleId, Role updatedRole);

    // Get a role by ID
    Task<Role> GetRoleByIdAsync(Guid roleId);

    // Get all roles
    Task<IEnumerable<Role>> GetAllRolesAsync();

    // Delete a role by ID
    Task<bool> DeleteRoleAsync(Guid roleId);

    // Assign a permission to a role
    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId);

    // Remove a permission from a role
    Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId);
}