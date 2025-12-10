namespace FileAnalysisService.UseCases.Analysis
{
    public sealed record AnalysisRequest(
        Guid WorkId,
        string StudentId,
        string AssignmentId
    );
}
