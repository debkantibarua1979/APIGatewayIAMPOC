using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.DTOs;
using ResourceProject.Entities.JoinEntities;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Services.Implementations;

public class RolePermissionService : IRolePermissionService
{
    private readonly AppDbContext _context;

        public RolePermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolePermissionDto>> GetPermissionsForRoleAsync(Guid roleId)
        {
            return await _context.RolePermissionRoles
                .Where(rpr => rpr.RoleId == roleId)
                .Select(rpr => new RolePermissionDto
                {
                    RoleId = rpr.RoleId,
                    PermissionId = rpr.PermissionId,
                    PermissionName = rpr.RolePermission.PermissionName
                })
                .ToListAsync();
        }

        public async Task<RolePermissionDto> GetPermissionForRoleAsync(Guid roleId, Guid permissionId)
        {
            var permission = await _context.RolePermissionRoles
                .Where(rpr => rpr.RoleId == roleId && rpr.PermissionId == permissionId)
                .Select(rpr => new RolePermissionDto
                {
                    RoleId = rpr.RoleId,
                    PermissionId = rpr.PermissionId,
                    PermissionName = rpr.RolePermission.PermissionName
                })
                .FirstOrDefaultAsync();

            return permission;
        }

        public async Task<bool> AddPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var rolePermissionRole = new RolePermissionRole
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            _context.RolePermissionRoles.Add(rolePermissionRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId)
        {
            var rolePermissionRole = await _context.RolePermissionRoles
                .FirstOrDefaultAsync(rpr => rpr.RoleId == roleId && rpr.PermissionId == permissionId);

            if (rolePermissionRole == null)
            {
                return false;
            }

            _context.RolePermissionRoles.Remove(rolePermissionRole);
            await _context.SaveChangesAsync();
            return true;
        }
    }
