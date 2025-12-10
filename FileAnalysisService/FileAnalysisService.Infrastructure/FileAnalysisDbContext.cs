using FileAnalysisService.Entities.Models;
using FileAnalysisService.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure
{
    public sealed class FileAnalysisDbContext : DbContext
    {
        public FileAnalysisDbContext(DbContextOptions<FileAnalysisDbContext> options)
            : base(options)
        {
        }

        public DbSet<FileHash> FileHashes => Set<FileHash>();
        public DbSet<Report> Reports => Set<Report>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileHash>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.WorkId)
                    .IsRequired();

                builder.Property(x => x.StudentId)
                    .HasConversion(
                        id => id.Value,
                        value => StudentId.Create(value))
                    .IsRequired();

                builder.Property(x => x.AssignmentId)
                    .HasConversion(
                        id => id.Value,
                        value => AssignmentId.Create(value))
                    .IsRequired();

                builder.Property(x => x.Hash)
                    .HasConversion(
                        h => h.Value,
                        value => FileContentHash.Create(value))
                    .IsRequired();

                builder.Property(x => x.UploadedAt)
                    .IsRequired();
            });

            modelBuilder.Entity<Report>(builder =>
            {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.WorkId)
                    .IsRequired();

                builder.Property(x => x.Similarity)
                    .HasConversion(
                        s => s.Value,
                        value => new Similarity(value))
                    .IsRequired();

                builder.Property(x => x.IsPlagiarism)
                    .IsRequired();

                builder.Property(x => x.WordCloudUrl);
            });
        }
    }
}
