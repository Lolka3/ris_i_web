using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RetakeRequest> RetakeRequests => Set<RetakeRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.ToTable("disciplines");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(item => item.Name).HasColumnName("name").IsRequired();
            entity.HasIndex(item => item.Name).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users", table =>
                table.HasCheckConstraint(
                    "users_role_check",
                    "\"role\" IN ('Student', 'Teacher', 'HeadOfDepartment')"));
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(item => item.Login).HasColumnName("login").IsRequired();
            entity.Property(item => item.FullName).HasColumnName("full_name").IsRequired();
            entity.Property(item => item.Role).HasColumnName("role").IsRequired();
            entity.Property(item => item.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.HasIndex(item => item.Login).IsUnique();
        });

        modelBuilder.Entity<RetakeRequest>(entity =>
        {
            entity.ToTable("retake_requests", table =>
            {
                table.HasCheckConstraint("retake_requests_number_check", "\"number\" > 0");
                table.HasCheckConstraint(
                    "retake_requests_title_check",
                    "char_length(title) BETWEEN 5 AND 80");
                table.HasCheckConstraint(
                    "retake_requests_description_check",
                    "char_length(description) BETWEEN 10 AND 500");
                table.HasCheckConstraint(
                    "retake_requests_status_check",
                    "\"status\" IN ('New', 'InProgress', 'Closed', 'Cancelled')");
            });
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(item => item.Number)
                .HasColumnName("number")
                .UseIdentityByDefaultColumn();
            entity.Property(item => item.Title).HasColumnName("title").IsRequired();
            entity.Property(item => item.Description).HasColumnName("description").IsRequired();
            entity.Property(item => item.DisciplineId).HasColumnName("discipline_id");
            entity.Property(item => item.Status).HasColumnName("status").IsRequired();
            entity.Property(item => item.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()");
            entity.Property(item => item.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(item => item.AssigneeUserId).HasColumnName("assignee_user_id");
            entity.Property(item => item.RetakeDate).HasColumnName("retake_date").HasColumnType("date");
            entity.Property(item => item.RetakeTime)
                .HasColumnName("retake_time")
                .HasColumnType("time without time zone");
            entity.Property(item => item.Audience).HasColumnName("audience");

            entity.HasOne(item => item.Discipline)
                .WithMany(item => item.RetakeRequests)
                .HasForeignKey(item => item.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(item => item.CreatedByUser)
                .WithMany(item => item.CreatedRequests)
                .HasForeignKey(item => item.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(item => item.AssigneeUser)
                .WithMany(item => item.AssignedRequests)
                .HasForeignKey(item => item.AssigneeUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(item => item.Number).IsUnique();
            entity.HasIndex(item => item.Status).HasDatabaseName("retake_requests_status_idx");
            entity.HasIndex(item => item.DisciplineId)
                .HasDatabaseName("retake_requests_discipline_id_idx");
        });
    }
}
