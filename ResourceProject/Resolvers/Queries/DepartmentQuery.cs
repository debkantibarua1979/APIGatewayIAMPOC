using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ResourceProject.Data;
using ResourceProject.Entities;

namespace ResourceProject.Resolvers.Queries;

public class DepartmentQuery
{
    [Authorize(Policy = "CanReadDepartment")]
    public async Task<List<Department>> GetDepartments([Service] AppDbContext context)
    {
        return await context.Departments.ToListAsync();
    }

    [Authorize(Policy = "CanReadDepartment")]
    public async Task<Department?> GetDepartmentById(Guid id, [Service] AppDbContext context)
    {
        return await context.Departments.FindAsync(id);
    }
}