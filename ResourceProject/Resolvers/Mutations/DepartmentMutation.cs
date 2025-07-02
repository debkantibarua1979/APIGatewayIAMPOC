using Microsoft.AspNetCore.Authorization;
using ResourceProject.Data;
using ResourceProject.Entities;

namespace ResourceProject.Resolvers.Mutations;

public class DepartmentMutation
{
    [Authorize(Policy = "CanWriteDepartment")]
    public async Task<Department> CreateDepartment(Department input, [Service] AppDbContext context)
    {
        context.Departments.Add(input);
        await context.SaveChangesAsync();
        return input;
    }

    [Authorize(Policy = "CanWriteDepartment")]
    public async Task<Department?> UpdateDepartment(Guid id, Department input, [Service] AppDbContext context)
    {
        var dept = await context.Departments.FindAsync(id);
        if (dept is null) return null;

        dept.Name = input.Name;
        dept.Description = input.Description;
        await context.SaveChangesAsync();
        return dept;
    }

    [Authorize(Policy = "CanDeleteDepartment")]
    public async Task<bool> DeleteDepartment(Guid id, [Service] AppDbContext context)
    {
        var dept = await context.Departments.FindAsync(id);
        if (dept is null) return false;

        context.Departments.Remove(dept);
        await context.SaveChangesAsync();
        return true;
    }
}