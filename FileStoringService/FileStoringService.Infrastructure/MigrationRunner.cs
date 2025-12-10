using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileStoringService.Infrastructure
{
    internal sealed class MigrationRunner(IServiceScopeFactory serviceScopeFactory) : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            FileStoringDbContext dbContext = scope.ServiceProvider.GetRequiredService<FileStoringDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
