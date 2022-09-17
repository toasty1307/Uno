using Microsoft.Extensions.DependencyInjection;
using Remora.HelpSystem.Services;

namespace Remora.HelpSystem;

public static class ServiceCollectionExtensions
{
    public static void AddHelpSystem(this IServiceCollection services)
    {
        services.AddScoped<ITreeWalker, TreeWalker>();
        
    }
}