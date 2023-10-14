using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Animals.DAL.Impl.Repository.Base;
using Animals.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animals.DAL.Impl.Tests.Base;

public class GenericRepositoryTests
{
    private readonly AnimalsContext _context; // Use your test database context.
    private readonly IGenericRepository<int, Dog> _repository;

    public GenericRepositoryTests()
    {
        // Get the connection string from the secrets configuration
        var connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AnimalsDBUnitTests;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=True;";

        var options = new DbContextOptionsBuilder<AnimalsContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new AnimalsContext(options);
        _repository = new DogRepository(_context);
        
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
    public async Task AddAsync_ShouldAddEntityToDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };

        // Act
        var addedDog = await _repository.AddAsync(dog);

        // Assert
        Assert.NotNull(addedDog);
        Assert.NotEqual(0, addedDog.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldRetrieveEntityFromDatabase()
    {
        // Arrange
        var dog = new Dog { Name = "TestDog", Color = "Brown", TailLength = 15, Weight = 20 };
        var addedDog = await _repository.AddAsync(dog);

        // Act
        var retrievedDog = await _repository.GetByIdAsync(addedDog.Id);

        // Assert
        Assert.NotNull(retrievedDog);
        Assert.Equal(addedDog.Id, retrievedDog.Id);
        Assert.Equal(dog.Name, retrievedDog.Name);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntityFromDatabase()
    {
        // Arrange
        var dog = await _repository.GetByIdAsync(1);

        // Act
        var result = await _repository.DeleteAsync(dog.Id);

        // Assert
        Assert.True(result);

        // Ensure that the dog is removed from the database
        var retrievedDog = await _repository.GetByIdAsync(dog.Id);
        Assert.Null(retrievedDog);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityInDatabase()
    {
        // Arrange
        var dog = await _repository.GetByIdAsync(1);
        dog.Name = "UpdatedDog";
        dog.Color = "Black";

        // Act
        var result = await _repository.UpdateAsync(dog);

        // Assert
        Assert.True(result);

        // Retrieve the dog from the database and check if it's updated
        var retrievedDog = await _repository.GetByIdAsync(dog.Id);
        Assert.NotNull(retrievedDog);
        Assert.Equal("UpdatedDog", retrievedDog.Name);
        Assert.Equal("Black", retrievedDog.Color);
    }

    [Fact]
    public void GetAllQueryable_ShouldReturnQueryable()
    {
        // Act
        var queryable = _repository.GetAllQueryable();

        // Assert
        Assert.NotNull(queryable);
        Assert.IsAssignableFrom<IQueryable<Dog>>(queryable);
    }
}



