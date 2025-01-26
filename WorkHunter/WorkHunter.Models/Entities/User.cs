using Microsoft.AspNetCore.Identity;

namespace WorkHunter.Models.Entities;

public sealed class User : IdentityUser
{
    public bool IsDeleted { get; set; }

    public required string Name { get; set; }
    
    public ICollection<UserRole>? UserRoles { get; set; }
}
