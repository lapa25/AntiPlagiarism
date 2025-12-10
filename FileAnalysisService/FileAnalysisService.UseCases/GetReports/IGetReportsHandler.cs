namespace FileAnalysisService.UseCases.GetReports
{
    public interface IGetReportsHandler
    {
        IReadOnlyList<GetReportsResponseItem> Handle(Guid workId);
    }
}
