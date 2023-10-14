using Animals.BLL.Abstract.Services;
using Animals.BLL.Impl.Mappers;
using Animals.BLL.Impl.Services;
using Animals.DAL.Abstract.Repository.Base;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Animals.BLL.Impl.Tests;

public class BllDependencyInstallerTests
{
    [Fact]
    public void InstallServices_AddsDogService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IDogService, DogService>();

        // Register a mock or a fake implementation of IUnitOfWork
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        services.AddScoped(_ => unitOfWorkMock.Object);

        // Create an instance of MapperConfiguration and IMapper
        var config = new MapperConfiguration(cfg =>
        {
            // Add any necessary mappings for your DogService
            cfg.AddProfile<MappersProfile>();
        });
        IMapper mapper = config.CreateMapper();

        // Register the IMapper in the service collection
        services.AddSingleton(mapper);

        // Act
        services.InstallServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dogService = serviceProvider.GetRequiredService<IDogService>();
        Assert.NotNull(dogService);
    }



    [Fact]
    public void InstallMappers_AddsMapper()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.InstallMappers();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        Assert.NotNull(mapper);
    }
}
