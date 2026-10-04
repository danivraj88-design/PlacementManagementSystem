using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlacementManagementSystem.Models;

namespace PlacementManagementSystem.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Student> Students { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<PlacementDrive> PlacementDrives { get; set; }

        public DbSet<JobOpening> JobOpenings { get; set; }

        public DbSet<PlacementApplication> PlacementApplications { get; set; }


        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // ==========================================
            // STUDENT -> APPLICATION USER
            // ==========================================
            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==========================================
            // COMPANY -> APPLICATION USER
            // ==========================================
            builder.Entity<Company>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==========================================
            // JOB OPENING -> COMPANY
            // ==========================================
            builder.Entity<JobOpening>()
                .HasOne(j => j.Company)
                .WithMany()
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==========================================
            // JOB OPENING -> PLACEMENT DRIVE
            // ==========================================
            builder.Entity<JobOpening>()
                .HasOne(j => j.PlacementDrive)
                .WithMany()
                .HasForeignKey(j => j.PlacementDriveId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==========================================
            // APPLICATION -> JOB OPENING
            // ==========================================
            builder.Entity<PlacementApplication>()
                .HasOne(a => a.JobOpening)
                .WithMany()
                .HasForeignKey(a => a.JobOpeningId)
                .OnDelete(DeleteBehavior.NoAction);


            // ==========================================
            // APPLICATION -> STUDENT
            // ==========================================
            builder.Entity<PlacementApplication>()
                .HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}