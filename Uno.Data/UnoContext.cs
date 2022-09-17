using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using Uno.Data.Entities;

namespace Uno.Data;

internal sealed class UnoContext : DbContext
{
    internal DbSet<GuildEntity> Guilds { get; set; }  = null!;

    public UnoContext(DbContextOptions<UnoContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Snowflake>().HaveConversion<SnowflakeConverter>();
        configurationBuilder.Properties<Snowflake?>().HaveConversion<NullableSnowflakeConverter>();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnoContext).Assembly);
    }
}