using Animals.BLL.Impl.Mappers;
using Animals.BLL.Impl.Services;
using Animals.DAL.Abstract.Repository.Base;
using Animals.Entities;
using Animals.Models;
using AutoMapper;
using Moq;

namespace Animals.BLL.Impl.Tests.Services;

public class DogServiceTests
{
    // Helper method to create a mocked IMapper
    private IMapper CreateMockMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappersProfile>(); // Assuming you have a mapper profile
        });

        return configuration.CreateMapper();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateDogModel()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapper = CreateMockMapper();
        var service = new DogService(unitOfWorkMock.Object, mapper);

        var dogModel = new DogModel
        {
            Name = "Test Dog",
            Color = "Brown",
            TailLength = 15,
            Weight = 20
        };

        var dogEntity = mapper.Map<Dog>(dogModel);

        unitOfWorkMock
            .Setup(u => u.DogRepository.AddAsync(It.IsAny<Dog>()))
            .ReturnsAsync(dogEntity);

        // Act
        var result = await service.CreateAsync(dogModel);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dogModel.Name, result.Name);
        Assert.Equal(dogModel.Color, result.Color);
        Assert.Equal(dogModel.TailLength, result.TailLength);
        Assert.Equal(dogModel.Weight, result.Weight);
    }

    [Fact]
    public async Task GetAllAsync_ShouldRetrieveAllDogModels()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapper = CreateMockMapper();
        var service = new DogService(unitOfWorkMock.Object, mapper);

        var entities = new List<Dog>
        {
            // Create Dog entities
        };
        var expectedModels = entities.Select(mapper.Map<DogModel>).ToList();

        unitOfWorkMock
            .Setup(u => u.DogRepository.GetAllAsync(It.IsAny<Func<Dog, bool>>()))
            .ReturnsAsync(entities);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedModels.Count, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldRetrieveDogModelById()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapper = CreateMockMapper();
        var service = new DogService(unitOfWorkMock.Object, mapper);

        var dogId = 1;
        var dogEntity = new Dog { Id = dogId, Name = "TestDog" };

        unitOfWorkMock
            .Setup(u => u.DogRepository.GetByIdAsync(dogId))
            .ReturnsAsync(dogEntity);

        // Act
        var result = await service.GetByIdAsync(dogId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dogId, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDogModel()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapper = CreateMockMapper();
        var service = new DogService(unitOfWorkMock.Object, mapper);

        var dogModel = new DogModel { Id = 1, Name = "UpdatedDog" };

        unitOfWorkMock
            .Setup(u => u.DogRepository.UpdateAsync(It.IsAny<Dog>()))
            .ReturnsAsync(true);

        // Act
        var result = await service.UpdateAsync(dogModel);

        // Assert
        Assert.True(result);

        // Verify that the mapper mapped the model to an entity
        unitOfWorkMock.Verify(u => u.DogRepository.UpdateAsync(It.Is<Dog>(d => d.Name == "UpdatedDog")));
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveDogModel()
    {
        // Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapper = CreateMockMapper();
        var service = new DogService(unitOfWorkMock.Object, mapper);

        var dogId = 1;

        unitOfWorkMock
            .Setup(u => u.DogRepository.DeleteAsync(dogId))
            .ReturnsAsync(true);

        // Act
        var result = await service.DeleteAsync(dogId);

        // Assert
        Assert.True(result);

        // Verify that the DeleteAsync method was called with the correct ID
        unitOfWorkMock.Verify(u => u.DogRepository.DeleteAsync(dogId));
    }


    [Fact]
    public void SortDogs_ShouldSortDogsAscending()
    {
        // Arrange
        // Create a mock IMapper
        var mapper = CreateMockMapper();

        var service = new DogService(null, mapper);
        var dogs = new List<DogModel>
        {
            new DogModel { Id = 3, Name = "Dog1" },
            new DogModel { Id = 1, Name = "Dog2" },
            new DogModel { Id = 2, Name = "Dog3" }
        };

        // Act
        var sortedDogs = service.SortDogs(dogs, "Name", true);

        // Assert
        Assert.NotNull(sortedDogs);
        Assert.Collection(sortedDogs,
            item => Assert.Equal("Dog1", item.Name),
            item => Assert.Equal("Dog2", item.Name),
            item => Assert.Equal("Dog3", item.Name)
        );
    }

    [Fact]
    public void Pagination_ShouldPaginateDogs()
    {
        // Arrange
        var service = new DogService(null, null);
        var dogs = new List<DogModel>
        {
            new DogModel { Id = 1 },
            new DogModel { Id = 2 },
            new DogModel { Id = 3 },
            new DogModel { Id = 4 }
        };

        // Act
        var paginatedDogs = service.Pagination(dogs, 2, 2);

        // Assert
        Assert.NotNull(paginatedDogs);
        Assert.Collection(paginatedDogs,
            item => Assert.Equal(3, item.Id),
            item => Assert.Equal(4, item.Id)
        );
    }
}
