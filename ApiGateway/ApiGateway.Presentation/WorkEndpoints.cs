using ApiGateway.UseCases;
using ApiGateway.UseCases.DTOS;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace ApiGateway.Presentation
{
    public static class WorkEndpoints
    {
        public static IEndpointRouteBuilder MapWorkEndpoints(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder group = app.MapGroup("/api/works")
                .WithTags("Works");

            MapUpload(group);
            MapGetReports(group);

            return app;
        }

        private static void MapUpload(RouteGroupBuilder group)
        {
            _ = group.MapPost("", UploadWork)
                .DisableAntiforgery()
                .WithName("UploadWorkThroughGateway")
                .WithSummary("Upload work and run plagiarism analysis")
                .WithDescription("Загрузка работы студентом и запуск проверки на плагиат")
                .Accepts<IFormFile>("multipart/form-data")
                .WithOpenApi();
        }

        private static void MapGetReports(RouteGroupBuilder group)
        {
            _ = group.MapGet("{workId:guid}/reports", GetReports)
                .WithName("GetReportsThroughGateway")
                .WithSummary("Get plagiarism reports for a work")
                .WithDescription("Получение отчётов по конкретной работе")
                .WithOpenApi();
        }

        private static async Task<IResult> UploadWork(
            IFormFile file,
            string studentId,
            string assignmentId,
            UploadWorkUseCase useCase,
            CancellationToken cancellationToken)
        {
            if (file is null || string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(assignmentId))
            {
                return Results.BadRequest("file, studentId, assignmentId are required");
            }

            await using Stream fileStream = file.OpenReadStream();

            UploadWorkCommand command = new(
                fileStream,
                file.FileName,
                file.ContentType ?? "application/octet-stream",
                studentId,
                assignmentId);

            UploadWorkResultDto result = await useCase.ExecuteAsync(command, cancellationToken);

            return Results.Created($"/api/works/{result.WorkId}", new
            {
                workId = result.WorkId,
                reportStatus = result.ReportStatus,
                report = result.Report
            });
        }

        private static async Task<IResult> GetReports(
            Guid workId,
            GetReportsUseCase useCase,
            CancellationToken cancellationToken)
        {
            string body = await useCase.ExecuteAsync(workId, cancellationToken);
            return Results.Content(body, "application/json");
        }
    }
}
