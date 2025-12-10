using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure
{
    internal sealed class MigrationRunner(IServiceScopeFactory serviceScopeFactory) : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            FileAnalysisDbContext dbContext = scope.ServiceProvider.GetRequiredService<FileAnalysisDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
