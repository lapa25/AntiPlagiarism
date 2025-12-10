using ApiGateway.UseCases.Abstractions;
using Microsoft.Extensions.Logging;

namespace ApiGateway.UseCases
{
    public sealed class GetReportsUseCase(
        IFileAnalysisGatewayClient fileAnalysis,
        ILogger<GetReportsUseCase> logger)
    {
        private readonly IFileAnalysisGatewayClient _fileAnalysis = fileAnalysis;
        private readonly ILogger<GetReportsUseCase> _logger = logger;

        public async Task<string> ExecuteAsync(Guid workId, CancellationToken cancellationToken = default)
        {
            return await _fileAnalysis.GetReportsRawAsync(workId, cancellationToken);
        }
    }
}
