using Microsoft.EntityFrameworkCore;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Data;

public interface IWorkHunterDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<WResponse> WResponses { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
