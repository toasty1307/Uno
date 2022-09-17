using System.ComponentModel.DataAnnotations;
using Remora.Rest.Core;
using Uno.Data.DTOs;

namespace Uno.Data.Entities;

internal sealed class GuildEntity
{
    [Key] public Snowflake Id { get; init; }

    public string? Prefix { get; private set; }

    internal GuildEntity()
    {
    }

    internal GuildEntity(GuildDTO dto)
    {
        Id = dto.Id;
        Prefix = dto.Prefix;
    }
}