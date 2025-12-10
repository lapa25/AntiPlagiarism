namespace FileAnalysisService.UseCases.Abstractions
{
    public interface IFileStoringClient
    {
        Stream DownloadFile(Guid workId);
    }
}
