using Microsoft.EntityFrameworkCore;
using WorkHunter.Models.Entities.Interviews;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Data;

public interface IWorkHunterDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<WResponse> WResponses { get; set; }

    DbSet<VideoInterviewFile> VideoInterviewFiles { get; set; }

    DbSet<UserTaskType> UserTaskTypes { get; set; }

    DbSet<UserTask> UserTasks { get; set; }

    DbSet<UserTask> UserSettings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
