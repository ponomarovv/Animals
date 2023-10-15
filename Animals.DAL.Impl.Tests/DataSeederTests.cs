using Animals.DAL.Impl.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Animals.DAL.Impl.Tests;

public class DataSeederTests
{
    private readonly AnimalsContext _context;
    private DbContextOptions<AnimalsContext> _options;
    private readonly string _connectionString;
    private ServiceCollection _services;

    public DataSeederTests()
    {
        // Get the connection string from the secrets configuration
        _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AnimalsDBUnitTests4;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=True;";

        _options = new DbContextOptionsBuilder<AnimalsContext>()
            .UseSqlServer(_connectionString)
            .Options;

        _context = new AnimalsContext(_options);

        _services = new ServiceCollection();
        _services.AddDbContext<AnimalsContext>(
            options => options.UseSqlServer(_connectionString));

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // var dog1 = new Dog { Name = "Neo", Color = "red and amber", TailLength = 22, Weight = 32 };
        // var dog2 = new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 };
        // var dog3 = new Dog { Name = "ThirdName", Color = "yellow", TailLength = 3, Weight = 20 };

        // _context.AddRange(dog1, dog2, dog3);

        _context.SaveChanges();
    }

    [Fact]
    public void SeedData_WithNewDatabase_SeedsData()
    {
        // Arrange
        using var scope = _services.BuildServiceProvider().CreateScope();
        _context.Database.EnsureDeleted();

        // Act
        _services.SeedData();

        // Assert
        var dbContext = scope.ServiceProvider.GetRequiredService<AnimalsContext>();
        var dogCount = dbContext.Dogs.ToList().Count;
        Assert.Equal(3, dogCount); // Check that 3 dogs were seeded

    }

    [Fact]
    public void SeedData_WithExistingDatabase_DoesNotSeedData()
    {
        // Arrange

        // Act
        _services.SeedData();

        // Assert
        using var scope = _services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AnimalsContext>();
        var dogCount = dbContext.Dogs.Count();
        Assert.Equal(0, dogCount); // No dogs should be seeded
    }
}

