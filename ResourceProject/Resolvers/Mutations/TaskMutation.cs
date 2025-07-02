using Microsoft.AspNetCore.Authorization;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Mutations;

public class TaskMutation
{
    private readonly ITaskService _taskService;

    public TaskMutation(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [Authorize(Policy = "CanWriteDepartment")]
    public async Task<Entities.Task> CreateTaskAsync(Entities.Task input)
    {
        return await _taskService.CreateAsync(input);
    }

    [Authorize(Policy = "CanWriteDepartment")]
    public async Task<Entities.Task> UpdateTaskAsync(Guid id, Entities.Task input)
    {
        input.Id = id;
        return await _taskService.UpdateAsync(input);
    }

    [Authorize(Policy = "CanDeleteDepartment")]
    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _taskService.DeleteAsync(id);
    }
}