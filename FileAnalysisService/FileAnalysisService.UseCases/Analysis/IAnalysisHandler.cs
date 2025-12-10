namespace FileAnalysisService.UseCases.Analysis
{
    public interface IAnalysisHandler
    {
        AnalysisResponse Handle(AnalysisRequest request);
    }
}
