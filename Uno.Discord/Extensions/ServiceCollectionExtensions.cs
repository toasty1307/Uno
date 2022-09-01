using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Remora.Discord.Hosting.Extensions;

namespace Uno.Discord.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder AddDiscordServices(this IHostBuilder host, string token)
    {
        host.AddDiscordService(_ => token);
        host.ConfigureServices(
            (context, collection) =>
            {
                
            }
        );
        return host;
    }
}