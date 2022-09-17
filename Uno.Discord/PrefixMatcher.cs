using System.Text.RegularExpressions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Commands.Services;
using Remora.Results;
using Uno.Data.Mediator;

namespace Uno.Discord;

[AutoConstructor]
internal sealed partial class PrefixMatcher : ICommandPrefixMatcher
{
    private static readonly Regex MentionRegex = new(@"^(<@!?(?<ID>\d+)> ?)", RegexOptions.Compiled);
    
    private readonly ICommandContext _context;
    private readonly IConfiguration _configuration;
    private readonly IDiscordRestUserAPI _userApi;
    private readonly IMediator _mediator;

    public async ValueTask<Result<(bool Matches, int ContentStartIndex)>> MatchesPrefixAsync(string content, CancellationToken ct = new())
    {
        if (string.IsNullOrEmpty(content))
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((false, 0));
        
        if (!_context.GuildID.IsDefined(out var guildId))
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, 0));

        var createGuildRequest = await _mediator.Send(new CreateGuild.Request
        {
            Id = guildId
        }, ct);
        
        var prefix = _configuration["Bot:DefaultPrefix"] ?? "uno ";
        if (createGuildRequest.IsDefined(out var guild) && guild.Prefix is not null)
            prefix = guild.Prefix;
        
        if (content.StartsWith(prefix))
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, prefix.Length));
        
        var selfResult = await _userApi.GetCurrentUserAsync(ct);
        
        if (!selfResult.IsSuccess)
            return Result<(bool Matches, int ContentStartIndex)>.FromError(selfResult.Error);
        
        var match = MentionRegex.Match(content);
        
        if (match.Success && match.Groups["ID"].Value == selfResult.Entity.ID.ToString())
            return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((true, match.Length));
        
        return Result<(bool Matches, int ContentStartIndex)>.FromSuccess((false, 0));
    }}