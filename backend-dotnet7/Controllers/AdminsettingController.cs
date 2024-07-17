using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsettingController : ControllerBase
    {
        private readonly IAdminsettingService _adminsettingService;
        public AdminsettingController(IAdminsettingService adminsettingService)
        {
            _adminsettingService = adminsettingService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<RegisterDto>>> GetAlls()
        {
            var result = await _adminsettingService.GetAll(User);

            return Ok(result);
        }
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(RegisterDto request)
        {
            var result = await _adminsettingService.UpdateUser(request , User);
            if (result == null)
                return NotFound("User not found");
            return Ok(result);
        }

    }
}
