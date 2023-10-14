using Animals.DAL.Abstract.Repository;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Animals.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animals.DAL.Impl.Tests.Repository;

public class DogRepositoryTests
{
    private readonly AnimalsContext _context; // Use your test database context.
    private readonly IDogRepository _dogRepository;

    public DogRepositoryTests()
    {
        // Get the connection string from the secrets configuration
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

    public void Dispose()
    {
        // Dispose of the test context after the tests are done.
        _context.Dispose();
    }

    [Fact]
    public async void AddDogAsync_ShouldAddDogToDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };

        // Act
        var addedDog = await _dogRepository.AddAsync(dog);

        // Assert
        Assert.NotNull(addedDog);
        Assert.NotEqual(0, addedDog.Id);
    }

    [Fact]
    public async void GetDogByIdAsync_ShouldRetrieveDogFromDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _dogRepository.AddAsync(dog);

        // Act
        var retrievedDog = await _dogRepository.GetByIdAsync(addedDog.Id);

        // Assert
        Assert.NotNull(retrievedDog);
        Assert.Equal(addedDog.Id, retrievedDog.Id);
        Assert.Equal(dog.Name, retrievedDog.Name);
    }

    [Fact]
    public async void DeleteDogAsync_ShouldRemoveDogFromDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _dogRepository.AddAsync(dog);

        // Act
        var result = await _dogRepository.DeleteAsync(addedDog.Id);

        // Assert
        Assert.True(result);

        // Ensure that the dog is removed from the database
        var retrievedDog = await _dogRepository.GetByIdAsync(addedDog.Id);
        Assert.Null(retrievedDog);
    }

    [Fact]
    public async void UpdateDogAsync_ShouldUpdateDogInDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _dogRepository.AddAsync(dog);

        // Update the dog's properties
        addedDog.Name = "UpdatedDog";
        addedDog.Color = "Black";

        // Act
        var result = await _dogRepository.UpdateAsync(addedDog);

        // Assert
        Assert.True(result);

        // Retrieve the dog from the database and check if it's updated
        var retrievedDog = await _dogRepository.GetByIdAsync(addedDog.Id);
        Assert.NotNull(retrievedDog);
        Assert.Equal("UpdatedDog", retrievedDog.Name);
        Assert.Equal("Black", retrievedDog.Color);
    }

    [Fact]
    public async void GetAllDogsAsync_ShouldRetrieveAllDogs()
    {
        // Arrange
        var dog1 = new Dog { Name = "Dog1", Color = "Brown", TailLength = 15, Weight = 20 };
        var dog2 = new Dog { Name = "Dog2", Color = "Black", TailLength = 12, Weight = 18 };
        var dog3 = new Dog { Name = "Dog3", Color = "White", TailLength = 18, Weight = 25 };

        // Add dogs to the database
        await _dogRepository.AddAsync(dog1);
        await _dogRepository.AddAsync(dog2);
        await _dogRepository.AddAsync(dog3);

        // Act
        var dogs = await _dogRepository.GetAllAsync(d => d.Weight > 19);

        // Assert
        Assert.NotNull(dogs);
        Assert.Equal(4, dogs.Count);
    }
}
