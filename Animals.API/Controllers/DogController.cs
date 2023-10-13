using Animals.DAL.Abstract.Repository.Base;
using Animals.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Animals.API.Controllers
{
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
        public async Task<ActionResult<List<Dog>>> GetAll(string? attribute, string? order, int? pageNumber, int? pageSize)
        {
            var result = await _uow.DogRepository.GetAllAsync(x => true);

            if (result.Count == 0) return BadRequest("There are no dogs in database");
            
            result = result.OrderBy(x => x.Name).ToList();
            
            return Ok(result);
        }
        
        // [HttpGet("/dogs")]
        // public ActionResult<List<Dog>> GetAll(string attribute, string order, int? pageNumber, int? pageSize)
        // {
        //     var query = _uow.DogRepository.GetAllQueryable();
        //
        //     if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(order))
        //     {
        //         query = query.OrderBy( x=> x.Name); // Dynamically sort based on attribute and order
        //     }
        //
        //     if (pageNumber.HasValue && pageSize.HasValue)
        //     {
        //         query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        //     }
        //
        //     var result = query.ToList();
        //
        //     if (result.Count == 0)
        //         return BadRequest("There are no dogs in the database");
        //
        //     return Ok(result);
        // }
    }
}
