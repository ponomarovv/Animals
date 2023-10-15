using Animals.API.Controllers;
using Animals.BLL.Abstract.Services;
using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.Dtos;
using Animals.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Animals.API.Tests;

public class DogControllerTests
{
    [Fact]
    public void Ping_ShouldReturnOkResultWithCorrectMessage()
    {
        // Arrange
        var dogServiceMock = new Mock<IDogService>();
        var mapperMock = new Mock<IMapper>();
        var controller = new DogController(dogServiceMock.Object, mapperMock.Object);

        // Act
        var result = controller.Ping();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;
        Assert.Equal(200, okResult?.StatusCode);
        Assert.Equal("Dogshouseservice.Version1.0.1", okResult?.Value);
    }


    [Fact]
    public async Task GetAll_ShouldReturnOkWhenNoDogsInDatabase()
    {
        // Arrange
        var dogServiceMock = new Mock<IDogService>();
        var mapperMock = new Mock<IMapper>();
        var controller = new DogController(dogServiceMock.Object, mapperMock.Object);

        dogServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DogModel>());

        // Act
        var result = await controller.GetAll(null, null, null, true);

        // Assert
        Assert.IsType<ActionResult<List<DogModel>>>(result);
        OkObjectResult? okResult = (OkObjectResult)result.Result;

        var message = (string)okResult.Value;
        Assert.Equal("There are no dogs in database", message);
    }

    [Fact]
    public async Task GetAll_ShouldReturnInternalServerErrorOnException()
    {
        // Arrange
        var dogServiceMock = new Mock<IDogService>();
        var mapperMock = new Mock<IMapper>();
        var controller = new DogController(dogServiceMock.Object, mapperMock.Object);

        dogServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await controller.GetAll(null, null, null, true);

        // Assert
        Assert.IsType<ObjectResult>(result.Result);
        var objectResult = (ObjectResult)result.Result;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task AddDog_ShouldReturnOkForValidData()
    {
        // Arrange
        var createDogDto = new CreateDogDto
        {
            Name = "NewDog",
            Color = "Yellow", // Set the color property
            Weight = 30, // Set the weight property
            TailLength = 1
        };

        var dogServiceMock = new Mock<IDogService>();
        dogServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DogModel>());
        dogServiceMock.Setup(s => s.CreateAsync(It.IsAny<DogModel>()))
            .ReturnsAsync(new DogModel
            {
                Id = 1,
                Name = "NewDog",
                Color = "Yellow", // Set the color property
                Weight = 30, // Set the weight property
                TailLength = 1
            });

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<DogModel>(It.IsAny<CreateDogDto>()))
            .Returns(new DogModel
            {
                Id = 1,
                Name = "NewDog",
                Color = "Yellow", // Set the color property
                Weight = 30, // Set the weight property
                TailLength = 1
            });

        var controller = new DogController(dogServiceMock.Object, mapperMock.Object);

        // Act
        var result = await controller.AddDog(createDogDto);

        // Assert
        var okResult = (OkObjectResult)result.Result;
        var dogModel = (DogModel)okResult.Value;
        Assert.Equal(1, dogModel.Id);
        Assert.Equal("NewDog", dogModel.Name);
        Assert.Equal("Yellow", dogModel.Color); // Assert color
        Assert.Equal(30, dogModel.Weight); // Assert weight
        Assert.Equal(1, dogModel.TailLength); // Assert weight
    }

    [Fact]
    public async Task GetAll_NoDogsInDatabase_ReturnsOkResultWithMessage()
    {
        // Arrange
        var dogServiceMock = new Mock<IDogService>();
        dogServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DogModel>());
        var mapperMock = new Mock<IMapper>();
        var controller = new DogController(dogServiceMock.Object, mapperMock.Object);

        // Act
        var result = await controller.GetAll(attribute: null, pageNumber: null, pageSize: null, isAscendingOrder: true);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var message = Assert.IsType<string>(okResult.Value);
        Assert.Equal("There are no dogs in database", message);
    }
}
