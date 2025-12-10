using FileStoringService.UseCases.Abstractions;
using Microsoft.Extensions.Configuration;

namespace FileStoringService.Infrastructure
{
    public sealed class LocalFileStorage : IFileStorage
    {
        private readonly string _rootPath;

        public LocalFileStorage(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"] ?? "/data/works";
            _ = Directory.CreateDirectory(_rootPath);
        }

        public string Save(Stream content, string fileName)
        {
            string id = Guid.NewGuid().ToString("N");
            string ext = Path.GetExtension(fileName);
            string path = Path.Combine(_rootPath, $"{id}{ext}");

            using FileStream fileStream = File.Create(path);
            content.CopyTo(fileStream);

            return path;
        }

        public Stream Get(string storagePath)
        {
            return !File.Exists(storagePath) ? throw new FileNotFoundException(storagePath) : (Stream)File.OpenRead(storagePath);
        }
    }
}
