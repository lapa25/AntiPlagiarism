using FileAnalysisService.Entities.Models;

namespace FileAnalysisService.UseCases.Abstractions
{
    public interface IFileAnalysisRepository
    {
        FileHash? FindPlagiarism(string hash, string studentId);
        void AddHash(FileHash hash);
        Report AddReport(Report report);
        IReadOnlyList<Report> GetReportsByWork(Guid workId);
    }
}
