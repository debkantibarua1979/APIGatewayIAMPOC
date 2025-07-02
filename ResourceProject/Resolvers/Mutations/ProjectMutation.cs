using Microsoft.AspNetCore.Authorization;
using ResourceProject.Entities;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Mutations;

public class ProjectMutation
{
    private readonly IProjectService _projectService;

    public ProjectMutation(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [Authorize(Policy = "CanWriteDepartment")]
    public async Task<Project> CreateProjectAsync(Project input)
    {
        return await _projectService.CreateAsync(input);
    }

    [Authorize(Policy = "CanWriteDepartment")]
    public async Task<Project> UpdateProjectAsync(Guid id, Project input)
    {
        input.Id = id;
        return await _projectService.UpdateAsync(input);
    }

    [Authorize(Policy = "CanDeleteDepartment")]
    public async Task<bool> DeleteProjectAsync(Guid id)
    {
        return await _projectService.DeleteAsync(id);
    }
}