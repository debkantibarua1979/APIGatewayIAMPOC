using ResourceProject.Entities;
using ResourceProject.Repositories.Interfaces;

namespace ResourceProject.Repositories.Implementations;

using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.Include(u => u.Role).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        var user = await _context.Users.FindAsync(userId);
        var role = await _context.Roles.FindAsync(roleId);

        if (user == null || role == null) return false;

        user.RoleId = roleId;
        user.Role = role;

        await _context.SaveChangesAsync();
        return true;
    }
}
