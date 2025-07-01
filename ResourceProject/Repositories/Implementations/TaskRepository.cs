namespace ResourceProject.Repositories.Implementations;


using ResourceProject.Entities;
using ResourceProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Task>> GetAllAsync()
    {
        return await _context.Tasks.Include(t => t.Project).ToListAsync();
    }

    public async Task<Task?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .Include<Task, Project>(t => t.Project)  // Explicitly specify types for Include
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Task> CreateAsync(Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<Task> UpdateAsync(Task task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}
