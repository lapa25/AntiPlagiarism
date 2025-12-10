namespace FileAnalysisService.UseCases.GetReports
{
    public sealed record GetReportsResponseItem(
    Guid ReportId,
    DateTime CreatedAt,
    bool IsPlagiarism,
    Guid? SourceWorkId,
    double Similarity,
    string? WordCloudUrl
);
}
