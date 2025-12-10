using FileStoringService.UseCases.Abstractions;

namespace FileStoringService.UseCases.GetWorkFile
{
    public sealed class GetWorkFileHandler(IWorkRepository works, IFileStorage fileStorage) : IGetWorkFileHandler
    {
        private readonly IWorkRepository _works = works;
        private readonly IFileStorage _fileStorage = fileStorage;

        public GetWorkFileResponse? Handle(Guid workId)
        {
            Entities.Models.Work? work = _works.Get(workId);
            if (work is null)
            {
                return null;
            }

            Stream stream = _fileStorage.Get(work.StoragePath);

            return new GetWorkFileResponse(stream, work.OriginalFileName);
        }
    }
}