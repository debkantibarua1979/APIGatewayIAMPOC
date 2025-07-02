using ResourceProject.DTOs;
using ResourceProject.Entities;

namespace ResourceProject.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid userId);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId);
    Task<bool> AssignPermissionToRoleAsync(Guid userId, Guid permissionId);
}