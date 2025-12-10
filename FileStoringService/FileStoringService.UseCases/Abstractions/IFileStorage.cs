namespace FileStoringService.UseCases.Abstractions
{
    public interface IFileStorage
    {
        string Save(Stream content, string fileName);
        Stream Get(string storagePath);
    }
}

