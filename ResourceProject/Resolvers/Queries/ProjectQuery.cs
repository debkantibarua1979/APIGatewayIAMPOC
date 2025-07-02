using Microsoft.AspNetCore.Authorization;
using ResourceProject.Entities;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Queries;

public class ProjectQuery
{
    private readonly IProjectService _projectService;

    public ProjectQuery(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [Authorize(Policy = "CanReadDepartment")]
    public async Task<List<Project>> GetProjectsAsync()
    {
        return await _projectService.GetAllAsync();
    }

    [Authorize(Policy = "CanReadDepartment")]
    public async Task<Project?> GetProjectByIdAsync(Guid id)
    {
        return await _projectService.GetByIdAsync(id);
    }
}