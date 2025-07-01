namespace ResourceProject.Repositories.Interfaces;
using ResourceProject.Entities;

public interface ITaskRepository
{
    Task<List<Task>> GetAllAsync();
    Task<Task?> GetByIdAsync(Guid id);
    Task<Task> CreateAsync(Task task);
    Task<Task> UpdateAsync(Task task);
    Task<bool> DeleteAsync(Guid id);
}