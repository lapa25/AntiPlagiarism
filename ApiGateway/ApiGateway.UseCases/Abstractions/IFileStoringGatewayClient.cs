using ApiGateway.UseCases.DTOS;

namespace ApiGateway.UseCases.Abstractions
{
    public interface IFileStoringGatewayClient
    {
        Task<WorkCreatedDto> UploadWorkAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string studentId,
            string assignmentId,
            CancellationToken cancellationToken = default);
    }
}
