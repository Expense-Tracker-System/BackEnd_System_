using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUserNameController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserUserNameService _userUserNameService;
        private readonly IGenerateResponseService _generateResponseService;

        public UserUserNameController(UserManager<ApplicationUser> userManager, 
            IUserUserNameService userUserNameConfirmService, 
            IGenerateResponseService generateResponseService)
        {
            _userManager = userManager;
            _userUserNameService = userUserNameConfirmService;
            _generateResponseService = generateResponseService;
        }

        [HttpPut]
        [Route("updateUserName")]
        [Authorize]
        public async Task<ActionResult<LoginServiceResponseDto>> updateUserNameAsync([FromBody] UpdateUserNameDto updateUserNameDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not Authorized");
                }

                var isExist = await _userManager.FindByIdAsync(userId);
                if(isExist is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User is not found");
                }

                var confirmUserName = await _userUserNameService.userUserNameConfirm(updateUserNameDto.UserName, User);
                if(confirmUserName is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Name confirm failed");
                }

                var isAlreadyExist = await _userUserNameService.userUserNameAlreadyExist(updateUserNameDto.NewUserName);
                if(isAlreadyExist is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Name is Already Exist");
                }

                isExist.UserName = updateUserNameDto.NewUserName;
                isExist.NormalizedUserName = _userManager.NormalizeName(updateUserNameDto.NewUserName);

                var updateResult = await _userManager.UpdateAsync(isExist);
                if (!updateResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Username updated failed.");
                }

                var newToken = await _generateResponseService.GenerateJwtTokenAsync(isExist);

                var roles = await _userManager.GetRolesAsync(isExist);
                var userInfo = _generateResponseService.GenerateUserInfoAsync(isExist, roles);

                return new LoginServiceResponseDto
                {
                    NewToken = newToken,
                    userInfo = userInfo
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
