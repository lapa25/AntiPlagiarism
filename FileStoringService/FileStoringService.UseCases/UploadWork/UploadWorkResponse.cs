namespace FileStoringService.UseCases.UploadWork
{
    public sealed record UploadWorkResponse(
        Guid WorkId,
        string StudentId,
        string AssignmentId,
        DateTime UploadedAt
    );
}
