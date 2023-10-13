using Animals.DAL.Impl.Context;
using Animals.Entities;
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
            Console.WriteLine("db is already created");
            
            return services;
        }

        Console.WriteLine("db is creating now");
        
        // Seed data only if the database was just created
        SeedInitialData(dbContext);
        
        return services;
    }

    private static void SeedInitialData(AnimalsContext dbContext)
    {
        var dog1 = new Dog { Name = "Neo", Color = "red and amber", TailLength = 22, Weight = 32 };
        var dog2 = new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 };
        var dog3 = new Dog { Name = "ThirdName", Color = "yellow", TailLength = 3, Weight = 33 };

        dbContext.AddRange(dog1, dog2, dog3);
        
        dbContext.SaveChanges();
    }
}
