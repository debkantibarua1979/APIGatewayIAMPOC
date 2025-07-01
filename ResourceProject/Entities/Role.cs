using ResourceProject.Entities.JoinEntities;

namespace ResourceProject.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Navigation property to Users (one-to-many relationship)
    public ICollection<User> Users { get; set; }  // A role can have many users

    // Navigation property to RolePermissions (many-to-many relationship)
    public ICollection<RolePermissionRole> RolePermissionRoles { get; set; }
}
