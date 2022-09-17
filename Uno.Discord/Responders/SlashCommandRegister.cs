using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Remora.Discord.API;
using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Commands.Services;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Uno.Discord.Responders;

[AutoConstructor]
internal sealed partial class SlashCommandRegisterer : IResponder<IReady>
{
    private readonly IConfiguration _config;
    private readonly SlashService _slash;
    private readonly IHostApplicationLifetime _lifetime;

    public async Task<Result> RespondAsync(IReady gatewayEvent, CancellationToken ct = default)
    {
        var slashResult = _slash.SupportsSlashCommands();

        if (!slashResult.IsSuccess)
        {
#if DEBUG
            Debugger.Break();
#else
            _lifetime.StopApplication();
#endif
        }

        var debugGuild = _config["Discord:DebugGuild"];
        Result result;
        if (debugGuild is not null && ulong.TryParse(debugGuild, out var debugGuildId))
        {
            result = await _slash.UpdateSlashCommandsAsync(DiscordSnowflake.New(debugGuildId), ct: ct);
            return result;
        }

        result = await _slash.UpdateSlashCommandsAsync(null, ct: ct);
        return result;
    }
}