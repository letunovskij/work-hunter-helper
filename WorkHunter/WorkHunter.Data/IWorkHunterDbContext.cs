using Microsoft.EntityFrameworkCore;
using WorkHunter.Models.Entities;
using WorkHunter.Models.Entities.WHunter;

namespace WorkHunter.Data;

public interface IWorkHunterDbContext
{
    DbSet<User> Users { get; set; }

    DbSet<WResponse> Responses { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
