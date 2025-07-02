using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Queries;

public class UserQuery
{
    private readonly IUserService _userService;

    public UserQuery(IUserService userService)
    {
        _userService = userService;
    }

    // Query to get all users
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        return await _userService.GetUsersAsync();
    }

    // Query to get a user by ID
    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        return await _userService.GetUserByIdAsync(userId);
    }
}
