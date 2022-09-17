using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Remora.Discord.API;
using Remora.Rest.Core;

namespace Uno.Data;

internal sealed class SnowflakeConverter : ValueConverter<Snowflake, ulong>
{
    private static readonly ConverterMappingHints DefaultHints = new(precision: 20, scale: 0);

    /// <summary>
    /// Initializes a new instance of the <see cref="SnowflakeConverter"/> class.
    /// </summary>
    public SnowflakeConverter()
        : base(sf => sf.Value, value => DiscordSnowflake.New(value), DefaultHints)
    {
    }
}

internal sealed class NullableSnowflakeConverter : ValueConverter<Snowflake?, ulong?>
{
    private static readonly ConverterMappingHints DefaultHints = new(precision: 20, scale: 0);

    /// <summary>
    /// Initializes a new instance of the <see cref="SnowflakeConverter"/> class.
    /// </summary>
    public NullableSnowflakeConverter()
        : base(
            sf => sf.HasValue
                ? sf.Value.Value
                : default,
            value => value.HasValue
                ? DiscordSnowflake.New(value.Value)
                : default, DefaultHints)
    {
    }
}