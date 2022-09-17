using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uno.Data.Services;

namespace Uno.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection collection)
    {
        collection.AddMemoryCache();
        collection.AddMediatR(typeof(UnoContext).Assembly);
        collection.AddLazyCache(_ =>
        {
            var service = new CachingService(CachingService.DefaultCacheProvider)
            {
                DefaultCachePolicy =
                {
                    DefaultCacheDurationSeconds = 60 * 5
                }
            };
            return service;
        });
        collection.AddPooledDbContextFactory<UnoContext>((provider, builder) =>
        {
            var config = provider.GetService<IConfiguration>() ?? throw new InvalidOperationException("Tried to create a dbcontext without any configuration");
            var connectionString = config["Data:ConnectionString"] ?? throw new InvalidOperationException("Connection string not found");
            builder.UseNpgsql(connectionString);
        });
        
        collection.AddHostedService<DbMigrationService>();
        
        return collection;
    }
}