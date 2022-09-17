// https://github.com/VelvetThePanda/RemoraHelpSystem/blob/master/VTP.Remora.Commands.HelpSystem/Services/TreeWalker.cs

using Remora.Commands.Services;
using Remora.Commands.Trees.Nodes;

namespace Remora.HelpSystem.Services;

[AutoConstructor]
public sealed partial class TreeWalker : ITreeWalker
{
    private readonly CommandService _commands;
    
    public IReadOnlyList<IChildNode> FindNodes(string? name, string? treeName = null)
    { 
        if (!_commands.TreeAccessor.TryGetNamedTree(treeName, out var tree))
            return Array.Empty<IChildNode>();

        if (string.IsNullOrEmpty(name))
            return tree.Root.Children;
        
        var stack = new Stack<string>(name.Split(' ').Reverse());
        
        return FindNodesCore(stack, tree.Root).ToArray();
    }

    private IEnumerable<IChildNode> FindNodesCore(Stack<string> stack, IParentNode parent)
    {
        GetNextToken(out var current);

        IParentNode? next = null;
        
        foreach (var child in parent.Children)
        {
            if (
                child.Key.Equals(current, StringComparison.OrdinalIgnoreCase) ||
                child.Aliases.Contains(current, StringComparer.OrdinalIgnoreCase)
            )
            {
                if (TokensRemain())
                {
                    if (child is not IParentNode nextLevel)
                        continue;

                    next = nextLevel;
                    break;
                }
                    
                yield return child;
            }
        }
        
        if (!TokensRemain() || next is null)
            yield break;
        
        foreach (var child in FindNodesCore(stack, next))
            yield return child;

        bool TokensRemain() => stack.TryPeek(out _);
        void GetNextToken(out string? popped) => stack.TryPop(out popped);
    }
}