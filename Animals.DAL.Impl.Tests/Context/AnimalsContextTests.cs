using Animals.DAL.Abstract.Repository;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Animals.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animals.DAL.Impl.Tests.Context;

public class AnimalsContextTests : IDisposable
{
    private readonly AnimalsContext _context; // Use your test database context.
    private readonly IDogRepository _dogRepository;

    public AnimalsContextTests()
    {
        var connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AnimalsDBUnitTests1;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=True;";

        var options = new DbContextOptionsBuilder<AnimalsContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new AnimalsContext(options);
        _dogRepository = new DogRepository(_context);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var dog1 = new Dog { Name = "Neo", Color = "red and amber", TailLength = 22, Weight = 32 };
        var dog2 = new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 };
        var dog3 = new Dog { Name = "ThirdName", Color = "yellow", TailLength = 3, Weight = 20 };

        _context.AddRange(dog1, dog2, dog3);

        _context.SaveChanges();
    }

    [Fact]
    public void Dogs_DbSet_Getter_ShouldNotBeNull()
    {
        // Arrange


        // Act
        var dogsDbSet = _context.Dogs;

        // Assert
        Assert.NotNull(dogsDbSet);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
    }
}
