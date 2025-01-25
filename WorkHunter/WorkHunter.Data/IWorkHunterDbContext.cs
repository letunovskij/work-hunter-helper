using Microsoft.EntityFrameworkCore;
using WorkHunter.Models.Entities;

namespace WorkHunter.Data;

public interface IWorkHunterDbContext
{
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
}
