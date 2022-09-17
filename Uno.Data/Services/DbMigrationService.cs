using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Uno.Data.Services;

[AutoConstructor]
internal sealed partial class DbMigrationService : BackgroundService
{
    private readonly ILogger<DbMigrationService> _logger;
    private readonly IDbContextFactory<UnoContext> _dbFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(stoppingToken); 
        _logger.LogInformation("Checking for database migrations...");
        var migrations = await db.Database.GetPendingMigrationsAsync(stoppingToken);
        if (migrations.Any())
        {
            _logger.LogInformation("Applying database migrations...");
            await db.Database.MigrateAsync(stoppingToken);
            _logger.LogInformation("Database migrations applied");
        }
        else
        {
            _logger.LogInformation("No database migrations to apply");
        }
    }
}