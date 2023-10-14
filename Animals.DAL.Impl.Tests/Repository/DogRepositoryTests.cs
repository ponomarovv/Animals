using Animals.DAL.Abstract.Repository;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Animals.DAL.Impl.Tests;

public class DogRepositoryTests
{
    private readonly AnimalsContext _context; // Use your test database context.
    private readonly IDogRepository _dogRepository;
    private IConfiguration _configuration;
    
    public DogRepositoryTests()
    {
        // Get the connection string from the secrets configuration
        var connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AnimalsDBUnitTests;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=True;";

            

        var options = new DbContextOptionsBuilder<AnimalsContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new AnimalsContext(options);
        _dogRepository = new DogRepository(_context);
    }

    [Fact]
    public void Test1()
    {
        // arrange
        var expected = 5;


        // act
        var actual = 2 + 3;

        // assert

        Assert.Equal(expected, actual);
        Assert.True(2 > 1);
    }


    [Fact]
    public void Test2()
    {
        Assert.True(2 > 1);
    }
}
