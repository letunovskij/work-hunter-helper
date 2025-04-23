using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunterUtils.Data;
using WorkHunterUtils.Models.Models;

namespace WorkHunterUtils
{
    public sealed class WorkHunterUtilsDb : DbContext, IWorkHunterUtilsDb
    {
        public WorkHunterUtilsDb() { }

        public WorkHunterUtilsDb(DbContextOptions<WorkHunterUtilsDb> options) : base(options)
        {
        }

        public DbSet<CurrencyHistory> CurrencyHistories { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => { });
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CurrencyHistory>(entity =>
            {
                entity.Property(x => x.CurrenciesValues)
                      .IsRequired()
                      //.HasColumnType("jsonb")
                      .HasMaxLength(3000);

                entity.Property(x => x.CreateDate)
                      .HasDefaultValue(DateTime.UtcNow);
            });
        }
    }
}
