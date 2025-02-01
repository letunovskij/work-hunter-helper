using Microsoft.AspNetCore.Identity;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Models.Entities.Users;

public sealed class User : IdentityUser
{
    public bool IsDeleted { get; set; }

    public required string Name { get; set; }

    public ICollection<UserRole>? UserRoles { get; set; }

    public ICollection<WResponse>? Responses { get; set; }
}
