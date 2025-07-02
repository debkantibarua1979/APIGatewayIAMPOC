using ResourceProject.Entities;

namespace ResourceProject.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
}
