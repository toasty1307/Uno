using Remora.Rest.Core;
using Uno.Data.Entities;

namespace Uno.Data.DTOs;

public sealed class GuildDTO
{
    public Snowflake Id { get; private set; }

    public string? Prefix { get; set; }

    internal GuildDTO(GuildEntity entity)
    {
        Id = entity.Id;
        Prefix = entity.Prefix;
    }
}