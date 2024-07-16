using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorAuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenerateResponseService _generateResponseService;
        private readonly ILogService _logService;

        public TwoFactorAuthenticationController(
            UserManager<ApplicationUser> userManager, 
            IGenerateResponseService generateResponseService,
            ILogService logService)
        {
            _userManager = userManager;
            _generateResponseService = generateResponseService;
            _logService = logService;
        }

        [HttpPut]
        [Route("Update2FA")]
        public async Task<IActionResult> Update2FA([FromBody] Update2FADto update2FADto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not Authorized");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if(user is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User is not Founded");
                }

                user.TwoFactorEnabled = update2FADto.twoFactor;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Update 2FA is Failed");
                }

                if(user.TwoFactorEnabled is true)
                {
                    await _logService.SaveNewLog(user.UserName, "Enabled 2FA");
                }

                if(user.TwoFactorEnabled is false)
                {
                    await _logService.SaveNewLog(user.UserName, "Disabled 2FA");
                }

                // get roles
                var roles = await _userManager.GetRolesAsync(user);

                // get user information
                var userInfo = _generateResponseService.GenerateUserInfoAsync(user,roles);

                // get new JWT
                var newToken = await _generateResponseService.GenerateJwtTokenAsync(user);

                var responseResult = new LoginServiceResponseDto
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Successfully update 2FA",
                    NewToken = newToken,
                    userInfo = userInfo,
                };

                return StatusCode(responseResult.StatusCode, responseResult);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
