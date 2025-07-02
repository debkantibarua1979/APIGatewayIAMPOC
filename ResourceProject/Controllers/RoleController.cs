using Microsoft.AspNetCore.Mvc;
using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        // Constructor injection for IRoleService
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Get all roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        // Get a role by Id
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRoleById(Guid roleId)
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            return Ok(role);
        }

        // Create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            if (createRoleDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var role = await _roleService.CreateRoleAsync(createRoleDto);
            if (role == null)
            {
                return BadRequest("Role creation failed.");
            }

            return CreatedAtAction(nameof(GetRoleById), new { roleId = role.Id }, role);
        }

        // Update role
        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            if (updateRoleDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var updatedRole = await _roleService.UpdateRoleAsync(roleId, updateRoleDto);
            if (updatedRole == null)
            {
                return NotFound("Role not found.");
            }

            return Ok(updatedRole);
        }

        // Delete role
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var result = await _roleService.DeleteRoleAsync(roleId);
            if (!result)
            {
                return NotFound("Role not found.");
            }
            return NoContent();  // 204 No Content response
        }

        // Assign a permission to a role
        [HttpPost("{roleId}/assign-permission/{permissionId}")]
        public async Task<IActionResult> AssignPermissionToRole(Guid roleId, Guid permissionId)
        {
            var result = await _roleService.AssignPermissionToRoleAsync(roleId, permissionId);
            if (!result)
            {
                return BadRequest("Failed to assign permission.");
            }
            return Ok("Permission assigned successfully.");
        }
    }