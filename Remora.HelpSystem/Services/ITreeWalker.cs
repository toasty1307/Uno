using Remora.Commands.Trees.Nodes;

namespace Remora.HelpSystem.Services;

public interface ITreeWalker
{
    public IReadOnlyList<IChildNode> FindNodes(string? name, string? treeName = null);
}