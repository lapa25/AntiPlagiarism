using FileAnalysisService.UseCases.Abstractions;

namespace FileAnalysisService.UseCases.GetReports
{
    public sealed class GetReportsHandler(IFileAnalysisRepository repository) : IGetReportsHandler
    {
        private readonly IFileAnalysisRepository _repository = repository;

        public IReadOnlyList<GetReportsResponseItem> Handle(Guid workId)
        {
            IReadOnlyList<Entities.Models.Report> reports = _repository.GetReportsByWork(workId);

            return [.. reports
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new GetReportsResponseItem(
                    r.Id,
                    r.CreatedAt,
                    r.IsPlagiarism,
                    r.SourceWorkId,
                    r.Similarity.Value,
                    r.WordCloudUrl))];
        }
    }
}
