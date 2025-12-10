using System.Net.Http.Headers;
using System.Net.Http.Json;
using ApiGateway.UseCases.Abstractions;
using ApiGateway.UseCases.DTOS;
using Microsoft.Extensions.Logging;


namespace ApiGateway.Infrastructure
{
    public sealed class FileStoringGatewayClient(HttpClient httpClient, ILogger<FileStoringGatewayClient> logger) : IFileStoringGatewayClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<FileStoringGatewayClient> _logger = logger;

        public async Task<WorkCreatedDto> UploadWorkAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string studentId,
            string assignmentId,
            CancellationToken cancellationToken = default)
        {
            using MultipartFormDataContent multipart = [];
            using StreamContent fileContent = new(fileStream);

            fileContent.Headers.ContentType = string.IsNullOrWhiteSpace(contentType)
                ? new MediaTypeHeaderValue("application/octet-stream")
                : MediaTypeHeaderValue.Parse(contentType);

            multipart.Add(fileContent, "file", fileName);
            multipart.Add(new StringContent(studentId), "studentId");
            multipart.Add(new StringContent(assignmentId), "assignmentId");

            using HttpResponseMessage response =
                await _httpClient.PostAsync("/works", multipart, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "FileStoringService returned {StatusCode} on upload",
                    response.StatusCode);

                throw new HttpRequestException(
                    $"FileStoringService returned {response.StatusCode}",
                    null,
                    response.StatusCode);
            }

            WorkCreatedDto? dto =
                await response.Content.ReadFromJsonAsync<WorkCreatedDto>(cancellationToken: cancellationToken);

            if (dto is null)
            {
                _logger.LogError("FileStoringService returned empty body on upload");
                throw new InvalidOperationException("FileStoringService returned empty body");
            }

            return dto;
        }
    }
}
