namespace FileStoringService.UseCases.GetWorkFile
{
    public sealed record GetWorkFileResponse(
        Stream Content,
        string FileName
    );
}
