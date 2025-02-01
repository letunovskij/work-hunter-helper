using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Data;

public sealed class WorkHunterDbContext : IdentityDbContext<
    User,
    IdentityRole,
    string,
    IdentityUserClaim<string>,
    UserRole,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>>, IWorkHunterDbContext

{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
                    warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    public WorkHunterDbContext() { }

    public WorkHunterDbContext(DbContextOptions<WorkHunterDbContext> options) : base(options)
    {
    }

    public DbSet<WResponse> WResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(x => x.Email)
                  .IsRequired()
                  .HasMaxLength(100);
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.HasKey(x => new { x.UserId, x.RoleId });

            entity.HasOne(x => x.Role)
                  .WithMany()
                  .HasForeignKey(x => x.RoleId)
                  .IsRequired();

            entity.HasOne(x => x.User)
                  .WithMany(x => x.UserRoles)
                  .HasForeignKey(x => x.UserId)
                  .IsRequired();
        });

        builder.Entity<WResponse>(entity =>
        {
            entity.HasOne(x => x.User)
                  .WithMany(x => x.Responses)
                  .HasForeignKey(x => x.UserId)
                  .IsRequired();

            entity.Property(x => x.Email).HasMaxLength(200);
            entity.Property(x => x.AnswerText).HasMaxLength(4000);
            entity.Property(x => x.Contact).HasMaxLength(800);
            entity.Property(x => x.VacancyUrl).HasMaxLength(400);
        });
    }
}
