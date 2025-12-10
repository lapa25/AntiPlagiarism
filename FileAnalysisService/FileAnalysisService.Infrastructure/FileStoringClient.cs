using FileAnalysisService.UseCases.Abstractions;


namespace FileAnalysisService.Infrastructure
{
    public sealed class FileStoringClient(HttpClient httpClient) : IFileStoringClient
    {
        private readonly HttpClient _httpClient = httpClient;

        public Stream DownloadFile(Guid workId)
        {
            HttpResponseMessage response = _httpClient
                .GetAsync($"/works/{workId}/file")
                .GetAwaiter()
                .GetResult();

            _ = response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
        }
    }
}
