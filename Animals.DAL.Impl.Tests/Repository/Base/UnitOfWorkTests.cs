using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Animals.DAL.Impl.Repository.Base;
using Animals.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Animals.DAL.Impl.Tests.Repository.Base;

public class UnitOfWorkTests
{
    private readonly AnimalsContext _context;
    private readonly IDogRepository _dogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private DbContextOptions<AnimalsContext> _options;

    public UnitOfWorkTests()
    {
        // Get the connection string from the secrets configuration
        var connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AnimalsDBUnitTests3;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=True;";

        _options = new DbContextOptionsBuilder<AnimalsContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new AnimalsContext(_options);
        _dogRepository = new DogRepository(_context);

        _unitOfWork = new UnitOfWork(_context, _dogRepository);

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
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task AddDogAsync_ShouldAddDogToDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };

        // Act
        var addedDog = await _unitOfWork.DogRepository.AddAsync(dog);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.NotNull(addedDog);
        Assert.NotEqual(0, addedDog.Id);
    }

    [Fact]
    public async Task GetDogByIdAsync_ShouldRetrieveDogFromDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _unitOfWork.DogRepository.AddAsync(dog);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var retrievedDog = await _unitOfWork.DogRepository.GetByIdAsync(addedDog.Id);

        // Assert
        Assert.NotNull(retrievedDog);
        Assert.Equal(addedDog.Id, retrievedDog.Id);
        Assert.Equal(dog.Name, retrievedDog.Name);
        Assert.Equal(dog.Color, retrievedDog.Color);
    }

    [Fact]
    public async Task DeleteDogAsync_ShouldRemoveDogFromDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _unitOfWork.DogRepository.AddAsync(dog);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var result = await _unitOfWork.DogRepository.DeleteAsync(addedDog.Id);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.True(result);

        // Ensure that the dog is removed from the database
        var retrievedDog = await _unitOfWork.DogRepository.GetByIdAsync(addedDog.Id);
        Assert.Null(retrievedDog);
    }

    [Fact]
    public async Task UpdateDogAsync_ShouldUpdateDogInDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _unitOfWork.DogRepository.AddAsync(dog);
        await _unitOfWork.SaveChangesAsync();

        // Update the dog's properties
        addedDog.Name = "UpdatedDog";
        addedDog.Color = "Black";

        // Act
        var result = await _unitOfWork.DogRepository.UpdateAsync(addedDog);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.True(result);

        // Retrieve the dog from the database and check if it's updated
        var retrievedDog = await _unitOfWork.DogRepository.GetByIdAsync(addedDog.Id);
        Assert.NotNull(retrievedDog);
        Assert.Equal("UpdatedDog", retrievedDog.Name);
        Assert.Equal("Black", retrievedDog.Color);
    }

    [Fact]
    public async Task GetAllDogsAsync_ShouldRetrieveAllDogs()
    {
        // Arrange
        var dog1 = new Dog { Name = "Dog1", Color = "Brown", TailLength = 15, Weight = 20 };
        var dog2 = new Dog { Name = "Dog2", Color = "Black", TailLength = 12, Weight = 18 };
        var dog3 = new Dog { Name = "Dog3", Color = "White", TailLength = 18, Weight = 25 };

        // Add dogs to the database
        await _unitOfWork.DogRepository.AddAsync(dog1);
        await _unitOfWork.DogRepository.AddAsync(dog2);
        await _unitOfWork.DogRepository.AddAsync(dog3);
        await _unitOfWork.SaveChangesAsync();

        // Act
        var dogs = await _unitOfWork.DogRepository.GetAllAsync(d => d.Weight > 19);

        // Assert
        Assert.NotNull(dogs);
        Assert.Equal(4, dogs.Count);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChanges()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        await _unitOfWork.DogRepository.AddAsync(dog);

        // Act
        await _unitOfWork.SaveChangesAsync();

        // Retrieve the dog from the database
        var retrievedDog = await _unitOfWork.DogRepository.GetByIdAsync(dog.Id);

        // Assert
        Assert.NotNull(retrievedDog);
        Assert.Equal(dog.Id, retrievedDog.Id);
        Assert.Equal(dog.Name, retrievedDog.Name);
        Assert.Equal(dog.Color, retrievedDog.Color);
    }
}
