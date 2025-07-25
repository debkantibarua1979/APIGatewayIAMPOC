using ResourceProject.Entities;
using ResourceProject.Repositories.Interfaces;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Services.Implementations;

using ResourceProject.Repositories;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Project> CreateAsync(Project project)
    {
        return await _repository.CreateAsync(project);
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        return await _repository.UpdateAsync(project);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
