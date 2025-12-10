using ApiGateway.UseCases.DTOS;

namespace ApiGateway.UseCases.Abstractions
{
    public interface IFileAnalysisGatewayClient
    {
        Task<AnalysisResultDto?> AnalysisAsync(
            AnalysisDto request,
            CancellationToken cancellationToken = default);

        Task<string> GetReportsRawAsync(
            Guid workId,
            CancellationToken cancellationToken = default);
    }
}
