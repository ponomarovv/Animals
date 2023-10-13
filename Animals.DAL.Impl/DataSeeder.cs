using Animals.DAL.Impl.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Animals.DAL.Impl;

public static class DataSeeder
{
    public static IServiceCollection SeedData(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AnimalsContext>();
        
        // Check if the database exists, and if it does, don't recreate it
        if (!dbContext.Database.EnsureCreated())
        {
            return services;
        }

        return services;
    }
}
