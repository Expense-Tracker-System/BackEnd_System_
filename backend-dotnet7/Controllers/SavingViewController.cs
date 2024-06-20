using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingViewController : ControllerBase
    {
        public readonly ISavingService _srvingService;
        public SavingViewController(ISavingService srvingService)
        {

            _srvingService = srvingService;

        }

       

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] SavingViewEntities modle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _srvingService.GetSarvingData(modle);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
