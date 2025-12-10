using FileAnalysisService.UseCases.GetReports;
using FileAnalysisService.UseCases.Analysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FileAnalysisService.Presentation
{
    public static class AnalysisEndpoints
    {
        public static IEndpointRouteBuilder MapAnalysisEndpoints(this IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints
                .MapGroup("/analysis")
                .WithTags("Analysis");

                _ = group.MapPost("run", (AnalysisRequest request, IAnalysisHandler handler) =>
                {
                    AnalysisResponse response = handler.Handle(request);
                    return Results.Ok(response);
                })
                    .WithName("Analysis")
                    .WithSummary("Run plagiarism analysis")
                    .WithOpenApi();

                _ = group.MapGet("works/{workId:guid}/reports", (Guid workId, IGetReportsHandler handler) =>
                {
                    IReadOnlyList<GetReportsResponseItem> reports = handler.Handle(workId);
                    return Results.Ok(reports);
                })
                    .WithName("GetReportsForWork")
                    .WithSummary("Get reports for work")
                    .WithOpenApi();

            return endpoints;
        }
    }
}
