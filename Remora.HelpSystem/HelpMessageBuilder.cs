using Remora.Discord.API.Abstractions.Objects;

namespace Remora.HelpSystem;

public sealed record HelpMessageBuilder
{
    public string Content { get; set; } = string.Empty;
    
    public List<IEmbed> Embeds { get; set; } = new();

    public List<IMessageComponent> Components { get; set; } = new();
}