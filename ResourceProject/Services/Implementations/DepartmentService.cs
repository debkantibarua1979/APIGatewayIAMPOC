using ResourceProject.Entities;
using ResourceProject.Repositories.Interfaces;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Services.Implementations;

using ResourceProject.Repositories;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Department> CreateAsync(Department department)
    {
        return await _repository.CreateAsync(department);
    }

    public async Task<Department> UpdateAsync(Department department)
    {
        return await _repository.UpdateAsync(department);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
