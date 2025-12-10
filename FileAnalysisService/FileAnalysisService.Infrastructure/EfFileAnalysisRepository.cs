using FileAnalysisService.Entities.Models;
using FileAnalysisService.Entities.ValueObjects;
using FileAnalysisService.UseCases.Abstractions;

namespace FileAnalysisService.Infrastructure
{
    public sealed class EfFileAnalysisRepository(FileAnalysisDbContext dbContext) : IFileAnalysisRepository
    {
        private readonly FileAnalysisDbContext _dbContext = dbContext;

        public FileHash? FindPlagiarism(string hash, string studentId)
        {
            return _dbContext.FileHashes
                .Where(x => x.Hash == FileContentHash.Create(hash) && x.StudentId != StudentId.Create(studentId))
                .OrderBy(x => x.UploadedAt)
                .FirstOrDefault();
        }

        public void AddHash(FileHash hash)
        {
            _ = _dbContext.FileHashes.Add(hash);
            _ = _dbContext.SaveChanges();
        }

        public Report AddReport(Report report)
        {
            _ = _dbContext.Reports.Add(report);
            _ = _dbContext.SaveChanges();
            return report;
        }

        public IReadOnlyList<Report> GetReportsByWork(Guid workId)
        {
            return [.. _dbContext.Reports
                .Where(x => x.WorkId == workId)
                .OrderByDescending(x => x.CreatedAt)];
        }
    }
}
