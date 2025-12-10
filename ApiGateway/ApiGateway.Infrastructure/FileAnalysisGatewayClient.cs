using ApiGateway.UseCases.Abstractions;
using ApiGateway.UseCases.DTOS;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace ApiGateway.Infrastructure
{
    public sealed class FileAnalysisGatewayClient(HttpClient httpClient, ILogger<FileAnalysisGatewayClient> logger) : IFileAnalysisGatewayClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<FileAnalysisGatewayClient> _logger = logger;

        public async Task<AnalysisResultDto?> AnalysisAsync(
            AnalysisDto request,
            CancellationToken cancellationToken = default)
        {
            using HttpResponseMessage response =
                await _httpClient.PostAsJsonAsync("/analysis/run", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "FileAnalysisService returned {StatusCode} for work {WorkId}",
                    response.StatusCode,
                    request.WorkId);

                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<AnalysisResultDto>(cancellationToken: cancellationToken);
        }

        public async Task<string> GetReportsRawAsync(
            Guid workId,
            CancellationToken cancellationToken = default)
        {
            using HttpResponseMessage response =
                await _httpClient.GetAsync($"/analysis/works/{workId}/reports", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "FileAnalysisService returned {StatusCode} when getting reports for work {WorkId}",
                    response.StatusCode,
                    workId);

                throw new HttpRequestException(
                    $"FileAnalysisService returned {response.StatusCode}",
                    null,
                    response.StatusCode);
            }

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
