namespace FileStoringService.UseCases.UploadWork
{
    public sealed record UploadWorkRequest(
        Stream FileStream,
        string FileName,
        string StudentId,
        string AssignmentId
    );
}
