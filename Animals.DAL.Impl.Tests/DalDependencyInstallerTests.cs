using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Animals.DAL.Impl.Repository.Base;
using Animals.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Animals.DAL.Impl.Tests;

public class DalDependencyInstallerTests
{
    private readonly AnimalsContext _context;
    private readonly IDogRepository _dogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private DbContextOptions<AnimalsContext> _options;

    public DalDependencyInstallerTests()
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
    
    [Fact]
    public void InstallRepositories_RegistersDogRepository()
    {
        // Arrange
        var services = new ServiceCollection();

        // Create a real instance of AnimalsContext using the options
        var realAnimalsContext = new AnimalsContext(_options);

        // Register the real instance in the service collection
        services.AddScoped(_ => realAnimalsContext);

        // Act
        DalDependencyInstaller.InstallRepositories(services);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dogRepository = serviceProvider.GetService<IDogRepository>();
        Assert.NotNull(dogRepository);
        Assert.IsType<DogRepository>(dogRepository);
    }


    [Fact]
    public void InstallRepositories_RegistersUnitOfWork()
    {
        // Arrange
        var services = new ServiceCollection();

        // Create a real instance of AnimalsContext using the options
        var realAnimalsContext = new AnimalsContext(_options);

        // Register the real instance in the service collection
        services.AddScoped(_ => realAnimalsContext);

        // Act
        DalDependencyInstaller.InstallRepositories(services);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        Assert.NotNull(unitOfWork);
        Assert.IsType<UnitOfWork>(unitOfWork);
    }
}
