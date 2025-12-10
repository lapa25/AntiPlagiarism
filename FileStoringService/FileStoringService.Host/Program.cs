using FileStoringService.Infrastructure;
using FileStoringService.Presentation;
using FileStoringService.UseCases;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddFileStoringInfrastructure(builder.Configuration); 
builder.Services.AddFileStoringUseCases();

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
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/api.json", "FileStoringService API"));

app.MapWorksEndpoints();

app.Run();
