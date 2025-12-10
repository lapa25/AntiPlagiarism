namespace FileStoringService.UseCases.UploadWork
{
    public interface IUploadWorkHandler
    {
        UploadWorkResponse Handle(UploadWorkRequest request);
    }
}
