using FileAnalysisService.UseCases.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileAnalysisInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            _ = services.AddDbContext<FileAnalysisDbContext>((sp, options) =>
            {
                IConfiguration config = sp.GetRequiredService<IConfiguration>();
                _ = options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            _ = services.AddScoped<IFileAnalysisRepository, EfFileAnalysisRepository>();

            _ = services.AddHttpClient<IFileStoringClient, FileStoringClient>(client =>
            {
                string baseUrl = configuration["FileStoringService:BaseUrl"]
                              ?? throw new InvalidOperationException("FileStoringService:BaseUrl not configured");
                client.BaseAddress = new Uri(baseUrl);
            });

            _ = services.AddSingleton<IWordCloudGenerator, WordCloudGenerator>();

            _ = services.AddHostedService<MigrationRunner>();

            return services;
        }
    }
}
