using Microsoft.AspNetCore.Authorization;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Queries;

public class TaskQuery
{
    private readonly ITaskService _taskService;

    public TaskQuery(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [Authorize(Policy = "CanReadDepartment")]
    public async Task<List<Entities.Task>> GetTasksAsync()
    {
        return await _taskService.GetAllAsync();
    }

    [Authorize(Policy = "CanReadDepartment")]
    public async Task<Entities.Task?> GetTaskByIdAsync(Guid id)
    {
        return await _taskService.GetByIdAsync(id);
    }
}