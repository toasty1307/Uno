using Remora.Commands.Trees.Nodes;
using Remora.HelpSystem;
using Remora.HelpSystem.Services;

namespace Uno.Discord;

[AutoConstructor]
internal sealed partial class HelpFormatter : IHelpFormatter
{
    private readonly ITreeWalker _treeWalker;

    public HelpMessageBuilder BuildHelp()
    {
        var commands = _treeWalker.FindNodes(null);
        var builder = new HelpMessageBuilder();

        builder.Content = string.Join(" ", commands.Select(x => x.Key));
        
        return builder;
    }

    public HelpMessageBuilder BuildCommandHelp(IChildNode command)
    {
        throw new NotImplementedException();
    }
}