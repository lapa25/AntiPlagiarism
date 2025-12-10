using FileStoringService.UseCases.GetWork;
using FileStoringService.UseCases.GetWorkFile;
using FileStoringService.UseCases.UploadWork;
using Microsoft.Extensions.DependencyInjection;

namespace FileStoringService.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFileStoringUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<IUploadWorkHandler, UploadWorkHandler>();
            _ = services.AddScoped<IGetWorkHandler, GetWorkHandler>();
            _ = services.AddScoped<IGetWorkFileHandler, GetWorkFileHandler>();

            return services;
        }
    }
}
