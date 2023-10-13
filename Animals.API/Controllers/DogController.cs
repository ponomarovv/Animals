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
        public async Task<ActionResult<List<Dog>>> GetAll()
        {
            var result = await _uow.DogRepository.GetAllAsync(x => true);
            if (result.Count == 0) return BadRequest("There are no dogs in database");

            return Ok(result);
        }
    }
}
