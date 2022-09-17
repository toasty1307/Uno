using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Commands.Results;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Commands.Attributes;
using Remora.Discord.Commands.Contexts;
using Remora.HelpSystem;
using Remora.HelpSystem.Services;
using Remora.Results;

namespace Uno.Discord.Commands.General;

[AutoConstructor]
internal sealed partial class HelpCommand : CommandGroup
{
    private readonly ICommandContext _context;
    private readonly IDiscordRestChannelAPI _channelApi;
    private readonly IDiscordRestInteractionAPI _interactionApi;
    private readonly ITreeWalker _treeWalker;
    private readonly IHelpFormatter _formatter;

    [Command("help", "h")]
    [Ephemeral]
    public async Task<IResult> Help([Greedy] string? command = null)
    {
        HelpMessageBuilder? builder;
        if (command is null)
        {
            builder = _formatter.BuildHelp();
        }
        else
        {
            var nodes = _treeWalker.FindNodes(command);
            if (nodes.Count == 0)
                return Result.FromError(new CommandNotFoundError(command));
            var node = nodes[0];
            builder = _formatter.BuildCommandHelp(node);
        }

        switch (_context)
        {
            case MessageContext:
                var messageResult = await _channelApi.CreateMessageAsync(_context.ChannelID, embeds: builder.Embeds, components: builder.Components, content: builder.Content);
                return messageResult;
            case InteractionContext interactionContext:
                var interactionResult =
                    await _interactionApi.EditOriginalInteractionResponseAsync(interactionContext.ApplicationID,
                        interactionContext.Token, embeds: builder.Embeds, components: builder.Components, content: builder.Content);
                return interactionResult;
            default:
                return Result.FromError(new InvalidOperationError("ICommandContext was not of any recognised type"));
        }
    }
}