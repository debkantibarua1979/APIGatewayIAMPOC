using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.DTOs;
using ResourceProject.Entities;
using ResourceProject.Entities.JoinEntities;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Services.Implementations;

public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            return await _context.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                })
                .ToListAsync();
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid roleId)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null) return null;

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            var role = new Role
            {
                Name = createRoleDto.Name
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<RoleDto> UpdateRoleAsync(Guid roleId, UpdateRoleDto updateRoleDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                return null;
            }

            role.Name = updateRoleDto.Name ?? role.Name;

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                return false;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissionRoles)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return false;
            }

            // Check if the permission already exists in the role's permissions
            var permissionExists = role.RolePermissionRoles
                .Any(rpr => rpr.PermissionId == permissionId);

            if (!permissionExists)
            {
                var rolePermissionRole = new RolePermissionRole
                {
                    RoleId = role.Id,
                    PermissionId = permissionId
                };

                _context.RolePermissionRoles.Add(rolePermissionRole);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }