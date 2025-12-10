using FileAnalysisService.Infrastructure;
using FileAnalysisService.Presentation;
using FileAnalysisService.UseCases;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddFileAnalysisInfrastructure(builder.Configuration);
builder.Services.AddFileAnalysisUseCases();

builder.Services.AddOpenApi("api", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers.Clear();
        document.Servers.Add(new OpenApiServer { Url = "/" });
        return Task.CompletedTask;
    });
});

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/api.json", "FileAnalysisService API"));

app.MapAnalysisEndpoints();

app.Run();
