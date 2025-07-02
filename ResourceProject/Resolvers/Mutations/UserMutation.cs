using ResourceProject.DTOs;
using ResourceProject.Services.Interfaces;

namespace ResourceProject.Resolvers.Mutations;

public class UserMutation
{
    private readonly IUserService _userService;

    public UserMutation(IUserService userService)
    {
        _userService = userService;
    }

    // Mutation to create a new user
    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        return await _userService.CreateUserAsync(createUserDto);
    }

    // Mutation to update a user
    public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto)
    {
        return await _userService.UpdateUserAsync(userId, updateUserDto);
    }

    // Mutation to delete a user
    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        return await _userService.DeleteUserAsync(userId);
    }
}
