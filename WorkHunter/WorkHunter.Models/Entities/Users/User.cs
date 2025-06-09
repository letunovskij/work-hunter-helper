using Microsoft.AspNetCore.Identity;
using WorkHunter.Models.Entities.Interviews;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Models.Entities.Users;

public sealed class User : IdentityUser<string>
{
    public bool IsDeleted { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime? DateBlocked { get; set; }

    public required string Name { get; set; }

    public ICollection<UserRole>? UserRoles { get; set; }

    public ICollection<WResponse>? Responses { get; set; }

    public ICollection<VideoInterviewFile>? VideoInterviewFiles { get; set; }

    public ICollection<UserTask>? UserTasks { get; set; }

    public ICollection<UserSetting>? Settings { get; set; }

    public ICollection<UserSession>? UserSessions { get; set; }
}
