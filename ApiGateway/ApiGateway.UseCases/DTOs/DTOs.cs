namespace ApiGateway.UseCases.DTOS
{
    public sealed record WorkCreatedDto(
        Guid WorkId,
        string StudentId,
        string AssignmentId,
        DateTime UploadedAt
    );

    public sealed record AnalysisDto(
        Guid WorkId,
        string StudentId,
        string AssignmentId
    );

    public sealed record AnalysisResultDto(
        Guid ReportId,
        Guid WorkId,
        bool IsPlagiarism,
        Guid? SourceWorkId,
        double Similarity
    );

    public sealed record UploadWorkResultDto(
        Guid WorkId,
        string ReportStatus,
        AnalysisResultDto? Report
    );
}
