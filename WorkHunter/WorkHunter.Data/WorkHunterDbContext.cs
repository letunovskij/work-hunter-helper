using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkHunter.Models.Entities.Interviews;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Settings;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;
using WorkHunter.Models.Enums;

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

    public DbSet<VideoInterviewFile> VideoInterviewFiles { get; set; }

    public DbSet<UserTaskType> UserTaskTypes { get; set; }

    public DbSet<UserTask> UserTasks { get; set; }

    public DbSet<UserTask> UserSettings { get; set; }

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

        builder.Entity<UserSetting>(entity =>
        {
            entity.ToTable("UserSettings");
            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(250);

            entity.Property(x => x.Value)
                  .HasColumnType("jsonb");

            entity.HasOne(x => x.User)
                  .WithMany(x => x.Settings)
                  .HasForeignKey(x => x.UserId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.UserId, x.Name })
                  .IsUnique();
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

            entity.Property(x => x.Status).HasDefaultValue(ResponseStatus.Open);
            entity.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        builder.Entity<VideoInterviewFile>(entity =>
        {
            entity.HasOne(x => x.CreatedBy)
                  .WithMany(x => x.VideoInterviewFiles)
                  .HasForeignKey(x => x.CreatedById)
                  .IsRequired();

            entity.HasOne(x => x.WResponse)
                  .WithMany(x => x.VideoInterviewFiles)
                  .HasForeignKey(x => x.WResponseId)
                  .IsRequired();

            entity.Property(x => x.Name).HasMaxLength(200);
            entity.Property(x => x.Path).HasMaxLength(400);
            entity.Property(x => x.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(x => x.UpdatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        builder.Entity<UserTaskType>(entity =>
        {
            entity.Property(x => x.TaskText).IsRequired();
            entity.Property(x => x.TaskName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.InitialNotificationSubject).HasMaxLength(200);
            entity.Property(x => x.Recipient).HasMaxLength(200);
        });

        builder.Entity<UserTask>(entity =>
        {
            entity.ToTable("UserTasks");
            entity.Property(x => x.Text).IsRequired();
            entity.Property(x => x.Completed).IsRequired(false);
            entity.Property(x => x.CompletionReason).HasMaxLength(500);

            entity.HasOne(x => x.Responsible)
                  .WithMany(x => x.UserTasks)
                  .HasForeignKey(x => x.ResponsibleId)
                  .IsRequired();

            entity.HasOne(x => x.WResponse)
                  .WithMany(x => x.UserTasks)
                  .HasForeignKey(x => x.WResponseId)
                  .IsRequired();

            entity.HasOne(x => x.Type)
                  .WithMany(x => x.UserTasks)
                  .HasForeignKey(x => x.TypeId)
                  .IsRequired();
        });
    }
}
