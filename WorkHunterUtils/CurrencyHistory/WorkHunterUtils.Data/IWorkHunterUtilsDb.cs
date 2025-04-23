using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunterUtils.Models.Models;

namespace WorkHunterUtils.Data
{
    public interface IWorkHunterUtilsDb
    {
        public DbSet<CurrencyHistory> CurrencyHistories { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
