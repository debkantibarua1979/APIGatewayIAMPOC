namespace ResourceProject.DTOs;

public class CreateUserDto
{
    public string Name { get; set; } // User's name
    public string Username { get; set; } // User's username
    public string Email { get; set; } // User's email address
    public string Designation { get; set; } // User's job designation
    public Guid RoleId { get; set; }
}