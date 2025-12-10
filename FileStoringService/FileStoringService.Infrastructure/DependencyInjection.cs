using FileStoringService.UseCases.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileStoringInfrastructure(
            this IServiceCollection services, ConfigurationManager configuration)
        {
            _ = services.AddDbContext<FileStoringDbContext>((sp, options) =>
            {
                IConfiguration config = sp.GetRequiredService<IConfiguration>();
                _ = options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            _ = services.AddScoped<IWorkRepository, EfWorkRepository>();
            _ = services.AddSingleton<IFileStorage, LocalFileStorage>();

            _ = services.AddHostedService<MigrationRunner>();

            return services;
        }
    }
}
