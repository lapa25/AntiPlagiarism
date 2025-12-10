using FileStoringService.Entities.Models;
using FileStoringService.Entities.ValueObjects;
using FileStoringService.UseCases.Abstractions;

namespace FileStoringService.UseCases.UploadWork
{
    public class UploadWorkHandler(IWorkRepository works, IFileStorage fileStorage) : IUploadWorkHandler
    {
        private readonly IWorkRepository _works = works;
        private readonly IFileStorage _fileStorage = fileStorage;

        public UploadWorkResponse Handle(UploadWorkRequest request)
        {
            string storagePath = _fileStorage.Save(request.FileStream, request.FileName);

            Work work = new()
            {
                Id = Guid.NewGuid(),
                StudentId = StudentId.Create(request.StudentId),
                AssignmentId = AssignmentId.Create(request.AssignmentId),
                UploadedAt = DateTime.UtcNow,
                StoragePath = storagePath,
                OriginalFileName = request.FileName
            };

            work = _works.Add(work);

            return new UploadWorkResponse(
                work.Id,
                work.StudentId.Value,
                work.AssignmentId.Value,
                work.UploadedAt);
        }
    }
}
