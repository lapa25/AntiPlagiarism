using FileStoringService.UseCases.GetWork;
using FileStoringService.UseCases.GetWorkFile;
using FileStoringService.UseCases.UploadWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace FileStoringService.Presentation
{
    public static class WorksEndpoints
    {
        public static IEndpointRouteBuilder MapWorksEndpoints(this IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints
                .MapGroup("/works")
                .WithTags("Works");

            _ = MapUpload(group);
            _ = MapGetWork(group);
            _ = MapDownload(group);

            return endpoints;
        }

        private static RouteGroupBuilder MapUpload(RouteGroupBuilder group)
        {
            _ = group.MapPost("", (HttpRequest request, IUploadWorkHandler handler) =>
            {
                IFormCollection form = request.ReadFormAsync().GetAwaiter().GetResult();
                IFormFile? file = form.Files["file"];
                string studentId = form["studentId"].ToString();
                string assignmentId = form["assignmentId"].ToString();

                if (file is null ||
                    string.IsNullOrWhiteSpace(studentId) ||
                    string.IsNullOrWhiteSpace(assignmentId))
                {
                    return Results.BadRequest("file, studentId, assignmentId are required");
                }

                using Stream fileStream = file.OpenReadStream();

                UploadWorkResponse response = handler.Handle(
                    new UploadWorkRequest(fileStream, file.FileName, studentId, assignmentId));

                return Results.Created($"/works/{response.WorkId}", response);
            })
                .WithName("UploadWork")
                .WithSummary("Upload a work")
                .WithDescription("Upload a work file with metadata")
                .WithOpenApi();

            return group;
        }

        private static RouteGroupBuilder MapGetWork(RouteGroupBuilder group)
        {
            _ = group.MapGet("{id:guid}", (Guid id, IGetWorkHandler handler) =>
            {
                GetWorkResponse? response = handler.Handle(id);
                return response is null ? Results.NotFound() : Results.Ok(response);
            })
                .WithName("GetWork")
                .WithSummary("Get work metadata")
                .WithDescription("Get metadata for a work by id")
                .WithOpenApi();

            return group;
        }

        private static RouteGroupBuilder MapDownload(RouteGroupBuilder group)
        {
            _ = group.MapGet("{id:guid}/file", (Guid id, IGetWorkFileHandler handler) =>
            {
                GetWorkFileResponse? response = handler.Handle(id);
                return response is null ? Results.NotFound() : Results.File(response.Content, "application/octet-stream", response.FileName);
            })
                .WithName("DownloadWorkFile")
                .WithSummary("Download work file")
                .WithDescription("Download file content for a work")
                .WithOpenApi();

            return group;
        }
    }
}