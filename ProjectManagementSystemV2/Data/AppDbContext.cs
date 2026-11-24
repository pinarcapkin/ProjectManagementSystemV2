using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemV2.Entities;

namespace ProjectManagementSystemV2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<TaskHistory> TaskHistories { get; set; } = null!;
        public DbSet<TaskTimeLog> TaskTimeLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            modelBuilder.Entity<ProjectMember>()
                .HasIndex(pm => new { pm.ProjectId, pm.UserId })
                .IsUnique();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Sistem genelinde tam yetkili yönetici" },
                new Role { Id = 2, Name = "ProjectManager", Description = "Proje ve ekip yöneticisi" },
                new Role { Id = 3, Name = "TeamMember", Description = "Standart ekip üyesi" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Kullanici",
                    Email = "admin@sistem.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    CreatedAt = new DateTime(2026, 1, 1),
                    IsActive = true,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1,
                    AssignedAt = new DateTime(2026, 1, 1),
                    IsActive = true
                }
            );

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 9001,
                    Title = "Örnek Proje: Web Sitesi Yenileme",
                    Description = "Şirketin kurumsal web sitesinin modern bir tasarımla yenilenmesi",
                    StartDate = new DateTime(2026, 1, 5),
                    EndDate = new DateTime(2026, 3, 1),
                    Status = ProjectStatus.Active,
                    UserId = 1,
                    CreatedAt = new DateTime(2026, 1, 1),
                    IsActive = true,
                    IsArchived = false,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 9001,
                    Title = "Ana sayfa tasarımını tamamla",
                    Description = "Yeni ana sayfa mockup'ına göre HTML/CSS kodlanacak",
                    ProjectId = 9001,
                    CreatedByUserId = 1,
                    AssignedToUserId = 1,
                    Status = ProjectTaskStatus.Todo,
                    Priority = TaskPriority.High,
                    DueDate = new DateTime(2026, 1, 20),
                    CreatedAt = new DateTime(2026, 1, 6)
                }
            );
        }
    }
}