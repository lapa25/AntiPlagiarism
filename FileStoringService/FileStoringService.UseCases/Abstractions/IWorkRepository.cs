using FileStoringService.Entities.Models;

namespace FileStoringService.UseCases.Abstractions
{
    public interface IWorkRepository
    {
        Work Add(Work work);
        Work? Get(Guid id);
    }
}

