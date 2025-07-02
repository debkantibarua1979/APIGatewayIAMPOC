using Microsoft.AspNetCore.Mvc;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolePermissionController : ControllerBase
{
    private readonly IRolePermissionService _rolePermissionService;

    // Constructor injection for IRolePermissionService
    public RolePermissionController(IRolePermissionService rolePermissionService)
    {
        _rolePermissionService = rolePermissionService;
    }

    // Get all permissions for a role
    [HttpGet("{roleId}/permissions")]
    public async Task<IActionResult> GetPermissionsForRole(Guid roleId)
    {
        var permissions = await _rolePermissionService.GetPermissionsForRoleAsync(roleId);
        if (permissions == null || permissions.Count() == 0)
        {
            return NotFound("No permissions found for the specified role.");
        }

        return Ok(permissions);
    }

    // Get a role's permission by permissionId
    [HttpGet("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> GetPermissionForRole(Guid roleId, Guid permissionId)
    {
        var permission = await _rolePermissionService.GetPermissionForRoleAsync(roleId, permissionId);
        if (permission == null)
        {
            return NotFound("Permission not found for the specified role.");
        }

        return Ok(permission);
    }

    // Add permission to role
    [HttpPost("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> AddPermissionToRole(Guid roleId, Guid permissionId)
    {
        var result = await _rolePermissionService.AddPermissionToRoleAsync(roleId, permissionId);
        if (!result)
        {
            return BadRequest("Failed to add permission to role.");
        }

        return Ok("Permission added successfully to the role.");
    }

    // Remove permission from role
    [HttpDelete("{roleId}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermissionFromRole(Guid roleId, Guid permissionId)
    {
        var result = await _rolePermissionService.RemovePermissionFromRoleAsync(roleId, permissionId);
        if (!result)
        {
            return BadRequest("Failed to remove permission from role.");
        }

        return Ok("Permission removed successfully from the role.");
    }
}