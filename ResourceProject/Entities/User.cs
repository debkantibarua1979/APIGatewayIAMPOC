namespace ResourceProject.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Designation { get; set; }

    // Foreign key to Role
    public Guid? RoleId { get; set; }
    public Role Role { get; set; }  // Navigation property to Role
    
}
