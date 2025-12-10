using ApiGateway.Infrastructure;
using ApiGateway.Presentation;
using ApiGateway.UseCases;
using ApiGateway.UseCases.Abstractions;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IFileStoringGatewayClient, FileStoringGatewayClient>((sp, client) =>
{
    IConfiguration config = sp.GetRequiredService<IConfiguration>();

    string baseUrl = config["FileStoringService:BaseUrl"]
                  ?? "http://filestoring-service:8080";

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<IFileAnalysisGatewayClient, FileAnalysisGatewayClient>((sp, client) =>
{
    IConfiguration config = sp.GetRequiredService<IConfiguration>();

    string baseUrl = config["FileAnalysisService:BaseUrl"]
                  ?? "http://fileanalysis-service:8080";

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<UploadWorkUseCase>();
builder.Services.AddScoped<GetReportsUseCase>();

builder.Services.AddOpenApi("api", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Servers.Clear();
        document.Servers.Add(new OpenApiServer { Url = "/" });
        return Task.CompletedTask;
    });
});

WebApplication app = builder.Build();

app.MapOpenApi();

app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/api.json", "API Gateway"));

app.MapWorkEndpoints();

app.Run();
