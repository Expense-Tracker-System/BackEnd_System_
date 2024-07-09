using backend_dotnet7.Core.Dtos;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SavingViewController : ControllerBase
    {
        public readonly ISavingService _srvingService;
        public SavingViewController(ISavingService srvingService)
        {

            _srvingService = srvingService;

        }

       

        [HttpPost]
        public async Task<IActionResult> PostSarvingDetails([FromBody] SavingViewEntities modle)
        {

            var userName = User.Identity.Name;

            var result = await _srvingService.PostSarvingDetails(modle, userName);
                return Ok(result);
        }

        [HttpGet]
        [Route("GetSavingDetails")]
        public async Task<IActionResult> GetSavingDetails([FromBody] savingViewrequestDTO request)
        {
            try
            {
                //var userName = User.Identity.Name;
                var userName = User.Identity.Name;
                var reuslt = await _srvingService.GetSavingDetails(userName, request);
                return Ok(reuslt);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
    }
}
