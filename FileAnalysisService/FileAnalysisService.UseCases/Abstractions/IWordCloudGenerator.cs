namespace FileAnalysisService.UseCases.Abstractions
{
    public interface IWordCloudGenerator
    {
        string? GenerateUrl(string text);
    }
}
