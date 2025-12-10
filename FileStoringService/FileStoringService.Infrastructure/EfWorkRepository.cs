using FileStoringService.Entities.Models;
using FileStoringService.UseCases.Abstractions;

namespace FileStoringService.Infrastructure
{
    public sealed class EfWorkRepository(FileStoringDbContext dbContext) : IWorkRepository
    {
        private readonly FileStoringDbContext _dbContext = dbContext;

        public Work Add(Work work)
        {
            _ = _dbContext.Works.Add(work);
            _ = _dbContext.SaveChanges();
            return work;
        }

        public Work? Get(Guid id)
        {
            return _dbContext.Works.FirstOrDefault(x => x.Id == id);
        }
    }
}