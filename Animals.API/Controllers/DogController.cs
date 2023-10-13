using Animals.BLL.Impl;
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

    // [HttpGet("/dogs")]
    // public async Task<ActionResult<List<Dog>>> GetAll(string? attribute, string? order, int? pageNumber, int? pageSize)
    // {
    //
    //     // DogService.SortDogs();
    //     var result = await _uow.DogRepository.GetAllAsync(x => true);
    //     
    //     if (result.Count == 0) return BadRequest("There are no dogs in database");
    //     
    //     // result = result.OrderBy(x => x.Name).ToList();
    //     result = result.OrderBy(x=>x.TailLength).ToList();
    //     
    //     return Ok(result);
    // }

    [HttpGet("/dogs")]
    public async Task<ActionResult<List<Dog>>> GetAll(string? attribute, string? order, int? pageNumber, int? pageSize)
    {
        try
        {
            var result = await _uow.DogRepository.GetAllAsync(x => true);

            if (result.Count == 0) return BadRequest("There are no dogs in database");


            if (!string.IsNullOrWhiteSpace(attribute) && !string.IsNullOrWhiteSpace(order))
            {
                result = DogService.SortDogs(result, attribute, order);
            }

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
}
