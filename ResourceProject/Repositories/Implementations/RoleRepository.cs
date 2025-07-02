using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.Entities;
using ResourceProject.Entities.JoinEntities;
using ResourceProject.Repositories.Interfaces;

namespace ResourceProject.Repositories.Implementations;

public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create a new role
        public async Task<Role> CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        // Update an existing role
        public async Task<Role> UpdateRoleAsync(Guid roleId, Role updatedRole)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissionRoles)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null) return null;

            role.Name = updatedRole.Name;
            role.RolePermissionRoles = updatedRole.RolePermissionRoles; // Update permissions

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }

        // Get a role by ID
        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Roles
                .Include(r => r.RolePermissionRoles)
                .ThenInclude(rpr => rpr.RolePermission)  // Including RolePermission in the RolePermissionRole join table
                .FirstOrDefaultAsync(r => r.Id == roleId);
        }

        // Get all roles
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissionRoles)
                .ThenInclude(rpr => rpr.RolePermission) // Including RolePermission in the RolePermissionRole join table
                .ToListAsync();
        }

        // Delete a role by ID
        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        // Assign a permission to a role
        public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissionRoles)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null) return false;

            // Check if the permission already exists for this role
            if (role.RolePermissionRoles.Any(rpr => rpr.PermissionId == permissionId))
                return false;

            var rolePermissionRole = new RolePermissionRole
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            _context.RolePermissionRoles.Add(rolePermissionRole);
            await _context.SaveChangesAsync();
            return true;
        }

        // Remove a permission from a role
        public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            var rolePermissionRole = await _context.RolePermissionRoles
                .FirstOrDefaultAsync(rpr => rpr.RoleId == roleId && rpr.PermissionId == permissionId);

            if (rolePermissionRole == null) return false;

            _context.RolePermissionRoles.Remove(rolePermissionRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }