using Animals.BLL.Abstract.Services;
using Animals.Dtos;
using Animals.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DogController : ControllerBase
{
    private readonly IDogService _dogService;
    private readonly IMapper _mapper;

    public DogController(IDogService dogService, IMapper mapper)
    {
        _dogService = dogService;
        _mapper = mapper;
    }


    [HttpGet("/ping")]
    public ActionResult<string> Ping()
    {
        var result = "Dogshouseservice.Version1.0.1";
        return Ok(result);
    }

    [HttpGet("/dogs")]
    public async Task<ActionResult<List<DogModel>>> GetAll(string? attribute, int? pageNumber, int? pageSize,
        bool? isAscendingOrder = true)
    {
        List<DogModel> result = new();

        try
        {
            result = await _dogService.GetAllAsync();

            if (result.Count == 0) return Ok("There are no dogs in database");

            result = _dogService.SortDogs(result, attribute, isAscendingOrder);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                result = _dogService.Pagination(result, pageNumber, pageSize);
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
    public async Task<ActionResult<DogModel>> AddDog([FromBody] CreateDogDto createDogDto)
    {
        if (createDogDto == null)
        {
            return BadRequest("Invalid dog data");
        }

        try
        {
            // check if we have the dog with same name in the db. it is a task requirement.
            var dogs = await _dogService.GetAllAsync();

            var exist = dogs.Any(x => x.Name == createDogDto.Name);
            if (exist) return BadRequest("Dog with the same name already exists in DB.");


            DogModel? dog = _mapper.Map<DogModel>(createDogDto);

            var addedDog = await _dogService.CreateAsync(dog);
            return Ok(addedDog);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
    
}
