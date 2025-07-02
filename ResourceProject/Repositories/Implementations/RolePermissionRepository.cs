using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.Entities;
using ResourceProject.Entities.JoinEntities;
using ResourceProject.Repositories.Interfaces;

namespace ResourceProject.Repositories.Implementations;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly AppDbContext _context;

    public RolePermissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RolePermission>> GetAllAsync()
    {
        return await _context.RolePermissions.ToListAsync();
    }

    public async Task<RolePermission?> GetByIdAsync(Guid id)
    {
        return await _context.RolePermissions.FindAsync(id);
    }

    public async Task<RolePermission> CreateAsync(RolePermission rolePermission)
    {
        _context.RolePermissions.Add(rolePermission);
        await _context.SaveChangesAsync();
        return rolePermission;
    }

    public async Task<RolePermission> UpdateAsync(RolePermission rolePermission)
    {
        _context.RolePermissions.Update(rolePermission);
        await _context.SaveChangesAsync();
        return rolePermission;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rolePermission = await _context.RolePermissions.FindAsync(id);
        if (rolePermission == null) return false;

        _context.RolePermissions.Remove(rolePermission);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignPermissionsToRoleAsync(Guid roleId, List<Guid> permissionIds)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissionRoles)  // Include the join entity RolePermissionRole
            .ThenInclude(rpr => rpr.RolePermission)  // Include RolePermission via the join entity
            .FirstOrDefaultAsync(r => r.Id == roleId);  // Get the Role by its Id
        if (role == null) return false;

        var permissions = await _context.RolePermissions.Where(p => permissionIds.Contains(p.Id)).ToListAsync();
        
        foreach (var permission in permissions)
        {
            // Create a RolePermissionRole (join entity)
            var rolePermissionRole = new RolePermissionRole
            {
                RoleId = role.Id,  // Set the RoleId for the RolePermissionRole
                PermissionId = permission.Id  // Set the PermissionId for the RolePermissionRole
            };

            // Add the RolePermissionRole to the Role's RolePermissions collection
            role.RolePermissionRoles.Add(rolePermissionRole);
        }
        
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> RemovePermissionsFromRoleAsync(Guid roleId, List<Guid> permissionIds)
    {
        var role = await _context.Roles
            .Include(r => r.RolePermissionRoles)
            .ThenInclude(rpr => rpr.RolePermission)  // Include the RolePermission entity through the join table
            .FirstOrDefaultAsync(r => r.Id == roleId);

        if (role == null)
        {
            throw new Exception("Role not found.");
        }

        // Get the RolePermissionRole entities that correspond to the permissions to remove
        var permissionsToRemove = role.RolePermissionRoles
            .Where(rpr => permissionIds.Contains(rpr.PermissionId))  // Filter by the foreign key (PermissionId)
            .ToList();

        // Remove the matching RolePermissionRole entities from the Role's RolePermissionRoles collection
        foreach (var rpr in permissionsToRemove)
        {
            role.RolePermissionRoles.Remove(rpr);
        }

        // Save the changes to the database
        await _context.SaveChangesAsync();
        return true;
    }
}