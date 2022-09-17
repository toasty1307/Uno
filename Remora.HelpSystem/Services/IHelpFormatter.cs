using Remora.Commands.Trees.Nodes;

namespace Remora.HelpSystem.Services;

public interface IHelpFormatter
{
    public HelpMessageBuilder BuildHelp();
    
    public HelpMessageBuilder BuildCommandHelp(IChildNode command);
}