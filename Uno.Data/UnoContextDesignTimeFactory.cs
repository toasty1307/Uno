using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Uno.Data;

internal sealed class UnoContextDesignTimeFactory : IDesignTimeDbContextFactory<UnoContext>
{
    public UnoContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<UnoContext>();
        builder.UseNpgsql();
        return new UnoContext(builder.Options);
    }
}