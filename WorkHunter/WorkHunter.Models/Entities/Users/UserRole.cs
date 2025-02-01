using Microsoft.AspNetCore.Identity;

namespace WorkHunter.Models.Entities.Users;

public sealed class UserRole : IdentityUserRole<string>
{
    public User? User { get; set; }

    public IdentityRole? Role { get; set; }
}
