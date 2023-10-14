using Animals.API.Dtos;
using Animals.BLL.Impl.Services;
using Animals.DAL.Abstract.Repository.Base;
using Animals.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DogController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public DogController(IUnitOfWork uow)
    {
        _uow = uow;
    }


    [HttpGet("/ping")]
    public ActionResult<string> Ping()
    {
        var result = "Dogshouseservice.Version1.0.1";
        return Ok(result);
    }

    [HttpGet("/dogs")]
    public async Task<ActionResult<List<Dog>>> GetAll(string? attribute, int? pageNumber, int? pageSize,
        bool? isAscendingOrder = true)
    {
        List<Dog> result = new();


        try
        {
            result = await _uow.DogRepository.GetAllAsync(x => true);

            if (result.Count == 0) return Ok("There are no dogs in database");

            result = DogService.SortDogs(result, attribute, isAscendingOrder);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                result = DogService.Pagination(result, pageNumber, pageSize);
            }


            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// POST method to add a new dog
    /// </summary>
    /// <param name="createDogDto">Dog object which will be added to the DB</param>
    /// <returns>Returns dog as a new added object</returns>
    [HttpPost("/dogs")]
    public async Task<ActionResult<Dog>> AddDog([FromBody] CreateDogDto createDogDto)
    {
        if (createDogDto == null)
        {
            return BadRequest("Invalid dog data");
        }

        try
        {
            // check if we have the dog with same name in the db. it is a task requirement.
            var dogs = await _uow.DogRepository.GetAllAsync(x => true);

            var exist = dogs.Any(x => x.Name == createDogDto.Name);
            if (exist) return BadRequest("Dog with the same name already exists in DB.");
            
            
            // todo insert mapper here.
            // Map the CreateDogDto to the Dog entity if needed
            var dog = new Dog()
            {
                Name = createDogDto.Name,
                Color = createDogDto.Color,
                TailLength = createDogDto.TailLength,
                Weight = createDogDto.Weight
            };

            var addedDog = await _uow.DogRepository.AddAsync(dog);
            return Ok(addedDog);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
}
