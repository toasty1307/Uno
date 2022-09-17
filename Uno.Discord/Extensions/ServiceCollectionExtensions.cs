using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Remora.Commands.Extensions;
using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.Commands.Extensions;
using Remora.Discord.Commands.Services;
using Remora.Discord.Gateway;
using Remora.Discord.Gateway.Extensions;
using Remora.Discord.Hosting.Extensions;
using Remora.HelpSystem;
using Remora.HelpSystem.Services;
using Uno.Discord.Commands.General;
using Uno.Discord.Responders;

namespace Uno.Discord.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder AddDiscordServices(this IHostBuilder host, string token)
    {
        host.AddDiscordService(_ => token);
        host.ConfigureServices(
            (context, collection) =>
            {
                collection.Configure<DiscordGatewayClientOptions>(options =>
                    options.Intents |= GatewayIntents.MessageContents);
                collection.AddHelpSystem();
                collection.AddScoped<IHelpFormatter, HelpFormatter>();
                collection.AddScoped<ICommandPrefixMatcher, PrefixMatcher>();

                collection.AddDiscordCommands(true);
                
                collection.AddCommandTree()
                    .WithCommandGroup<HelpCommand>()
                    .Finish();

                collection.AddResponder<SlashCommandRegisterer>();
            }
        );
        return host;
    }
}