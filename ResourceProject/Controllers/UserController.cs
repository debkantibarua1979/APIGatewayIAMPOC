using Microsoft.AspNetCore.Mvc;
using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor injection for UserService
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Get all users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // Get a single user by Id
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        // Create a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (createUserDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var user = await _userService.CreateUserAsync(createUserDto);
            if (user == null)
            {
                return BadRequest("User creation failed.");
            }

            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
        }

        // Update user details
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var updatedUser = await _userService.UpdateUserAsync(userId, updateUserDto);
            if (updatedUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(updatedUser);
        }

        // Delete a user
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
            {
                return NotFound("User not found.");
            }
            return NoContent();  // 204 No Content response
        }

        // Assign a role to a user
        [HttpPost("{userId}/assign-role/{roleId}")]
        public async Task<IActionResult> AssignRoleToUser(Guid userId, Guid roleId)
        {
            var result = await _userService.AssignRoleToUserAsync(userId, roleId);
            if (!result)
            {
                return BadRequest("Failed to assign role.");
            }
            return Ok("Role assigned successfully.");
        }

        // Remove a role from a user
        [HttpPost("{userId}/remove-role/{roleId}")]
        public async Task<IActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            var result = await _userService.RemoveRoleFromUserAsync(userId, roleId);
            if (!result)
            {
                return BadRequest("Failed to remove role.");
            }
            return Ok("Role removed successfully.");
        }

        // Assign a permission to a role (if relevant to your system)
        [HttpPost("{userId}/assign-permission/{permissionId}")]
        public async Task<IActionResult> AssignPermissionToRole(Guid userId, Guid permissionId)
        {
            var result = await _userService.AssignPermissionToRoleAsync(userId, permissionId);
            if (!result)
            {
                return BadRequest("Failed to assign permission.");
            }
            return Ok("Permission assigned successfully.");
        }
    }
