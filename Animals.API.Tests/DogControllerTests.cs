using Animals.API.Controllers;
using Animals.BLL.Abstract.Services;
using Animals.BLL.Impl;
using Animals.BLL.Impl.Services;
using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository;
using Animals.Dtos;
using Animals.Entities;
using Animals.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        var okResult = (OkObjectResult)result.Result!;
        var dogModel = (DogModel)okResult.Value!;
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

    [Fact]
    public async Task GetAll_3DogsInDatabase_Returns3Dogs()
    {
        // Arrange
        var services = new ServiceCollection();
       
        services.InstallRepositories();
        services.InstallMappers();
        services.InstallServices();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
       

       
        
        var connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=AnimalsDBUnitTests11;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=True;";

        var options = new DbContextOptionsBuilder<AnimalsContext>()
            .UseSqlServer(connectionString)
            .Options;
        
        services.AddDbContext<AnimalsContext>(
            options => options.UseSqlServer(connectionString));
        
        var serviceProvider = services.BuildServiceProvider();
        
        using var scope = serviceProvider.CreateScope();
        var _context = scope.ServiceProvider.GetRequiredService<AnimalsContext>();

        
        var uow = serviceProvider.GetService<IUnitOfWork>();
        var dogService = serviceProvider.GetService<IDogService>();
        var dogRepository = serviceProvider.GetService<IDogRepository>();
        var mapper = serviceProvider.GetService<IMapper>();

        var dogController = new DogController(dogService, mapper); 
        

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var dog1 = new Dog { Name = "Neo", Color = "red and amber", TailLength = 22, Weight = 32 };
        var dog2 = new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 };
        var dog3 = new Dog { Name = "ThirdName", Color = "yellow", TailLength = 3, Weight = 20 };

        _context.AddRange(dog1, dog2, dog3);

        _context.SaveChanges();

        
        //
        //
        
        
        //
        // var dogServiceMock = new Mock<IDogService>();
        // dogServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<DogModel>());
        // var mapperMock = new Mock<IMapper>();
        // var controller = new DogController(dogServiceMock.Object, mapperMock.Object);

        // Act
        var r =  dogController.GetAll(attribute: null, pageNumber: null, pageSize: null, isAscendingOrder: true).Result.Result;
        OkObjectResult? some = r as OkObjectResult ;
        var a =some.Value;
        //
        var list =  some;
        
        var actualresult = list.Value;
        
        // if (result is OkObjectResult okResult)
        // {
        //     // Access the value from the OkObjectResult
        //     var data = okResult.Value;
        //
        //     // Now you can work with the data, which should be the list of DogModel
        //     if (data is List<DogModel> dogModels)
        
        // Assert
 
        Assert.Equal(3, 33);

        Console.WriteLine("hi");
    }
}
