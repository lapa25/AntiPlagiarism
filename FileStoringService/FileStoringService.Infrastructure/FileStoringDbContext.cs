using FileStoringService.Entities.Models;
using FileStoringService.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure
{
    public sealed class FileStoringDbContext : DbContext
    {
        public FileStoringDbContext(DbContextOptions<FileStoringDbContext> options)
            : base(options)
        {
        }

        public DbSet<Work> Works => Set<Work>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Work>(builder =>
            {
                builder.HasKey(w => w.Id);

                builder.Property(w => w.StudentId)
                    .HasConversion(
                        id => id.Value,                  
                        value => StudentId.Create(value) 
                    )
                    .IsRequired();

                builder.Property(w => w.AssignmentId)
                    .HasConversion(
                        id => id.Value,
                        value => AssignmentId.Create(value)
                    )
                    .IsRequired();

                builder.Property(w => w.StoragePath)
                    .IsRequired();

                builder.Property(w => w.OriginalFileName)
                    .IsRequired();

                builder.Property(w => w.UploadedAt)
                    .IsRequired();
            });
        }
    }
}

