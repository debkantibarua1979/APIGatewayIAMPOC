using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.DTOs;
using ResourceProject.Entities;
using ResourceProject.Entities.JoinEntities;
using ResourceProject.Repositories.Interfaces;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Services.Implementations;

public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.UserName,
                    Email = u.Email,
                    Designation = u.Designation,
                    RoleName = u.Role.Name
                })
                .ToListAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Designation = user.Designation,
                RoleName = user.Role.Name
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == createUserDto.RoleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            var user = new User
            {
                Name = createUserDto.Name,
                UserName = createUserDto.Username,
                Email = createUserDto.Email,
                Designation = createUserDto.Designation,
                RoleId = createUserDto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Designation = user.Designation,
                RoleName = role.Name
            };
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            user.Name = updateUserDto.Name ?? user.Name;
            user.Email = updateUserDto.Email ?? user.Email;
            user.Designation = updateUserDto.Designation ?? user.Designation;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.UserName,
                Email = user.Email,
                Designation = user.Designation,
                RoleName = user.Role.Name
            };
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
            {
                return false;
            }

            user.RoleId = roleId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.RoleId != roleId)
            {
                return false;
            }

            user.RoleId = null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignPermissionToRoleAsync(Guid userId, Guid permissionId)
        {
            // Assuming the user has a role already assigned
            var user = await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Role == null)
            {
                return false;
            }

            var rolePermission = new RolePermissionRole
            {
                RoleId = user.RoleId ?? Guid.Empty,  // User’s role
                PermissionId = permissionId
            };

            _context.RolePermissionRoles.Add(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }
    }