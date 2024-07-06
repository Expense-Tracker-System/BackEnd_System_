using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeactivateUserAccountController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly ApplicationDbContext _context;
        public readonly ILogService _loggService;

        public DeactivateUserAccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, ILogService logService)
        {
            _userManager = userManager;
            _context = applicationDbContext;
            _loggService = logService;
        }

        [HttpPost]
        [Route("createDeactivateRequest")]
        public async Task<IActionResult> CreateDeactivateRequest([FromBody] DeactivateUserAccountDto deactivateUserAccountDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(userId is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User not Authorized yet");
                }

                var isExist = await _userManager.FindByIdAsync(userId);

                if(isExist == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User can't found");
                }

                DeactivateUserAccount deactivateUserAccount = new DeactivateUserAccount()
                {
                    UserId = isExist.Id,
                    Message = deactivateUserAccountDto.Message,
                    Date = deactivateUserAccountDto.Date
                };

                await _context.DeactivateUserAccounts.AddAsync(deactivateUserAccount);
                await _context.SaveChangesAsync();
                await _loggService.SaveNewLog(isExist.UserName, "Deactivate Account");

                return StatusCode(StatusCodes.Status200OK, "Deactivate request sended Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
