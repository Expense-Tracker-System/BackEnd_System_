using backend_dotnet7.Core.Entities.CreateServing;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServingController : ControllerBase
    {
        private readonly IServingService _servingService;

        public ServingController(IServingService servingService)
        {
            _servingService = servingService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDe([FromBody] ServingEntity serving)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var addedServing = await _servingService.AddServing(serving);
                return Ok(addedServing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult GetServings()
        {
            try
            {
                var servings = _servingService.GetServings();
                return Ok(servings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServingById(int id)
        {
            try
            {
                var serving = await _servingService.GetServingById(id);
                if (serving == null)
                {
                    return NotFound();
                }
                return Ok(serving);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteServing(int id)
        {
            try
            {
                var deleted = _servingService.DeleteServing(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
