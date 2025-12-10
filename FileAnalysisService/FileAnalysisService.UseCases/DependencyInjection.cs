using FileAnalysisService.UseCases.GetReports;
using FileAnalysisService.UseCases.Analysis;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalysisService.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileAnalysisUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<IAnalysisHandler, AnalysisHandler>();
            _ = services.AddScoped<IGetReportsHandler, GetReportsHandler>();
            return services;
        }
    }
}
