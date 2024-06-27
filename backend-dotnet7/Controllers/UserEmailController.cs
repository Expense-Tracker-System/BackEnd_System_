using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Controllers
{
    public class UserEmailController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserEmailService _userEmailService;
        private readonly ILogger<UserEmailController> _userLogger;

        public UserEmailController(UserManager<ApplicationUser> userManager, IUserEmailService userEmailService)
        {
            _userManager = userManager;
            _userEmailService = userEmailService;
        }

        [HttpPut]
        [Route("updateUserEmail")]
        [Authorize]
        public async Task<ActionResult<UpdateUserEmailResponseDto>> updateUserEmail([FromBody] UpdateUserEmailDto updateUserEmailDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not Authorized");
                }

                var isExists = await _userManager.FindByIdAsync(userId);
                if (isExists is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Can't find User");
                }

                if(updateUserEmailDto.Email is null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Email can't be null");
                }

                var isValid = await _userEmailService.EmailValidation(updateUserEmailDto.Email);

                if (!isValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Email is invalid");
                }

                var isUnique = await _userEmailService.IsEmailUnique(updateUserEmailDto.Email);

                if(isUnique is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Email is not Unique");
                }

                isExists.Email = updateUserEmailDto.Email;

                var result = await _userManager.UpdateAsync(isExists);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return StatusCode(StatusCodes.Status400BadRequest, errors);
                }

                var userInfoObject = new UserInfoResult
                {
                    Id = isExists.Id,
                    FirstName = isExists.FirstName,
                    LastName = isExists.LastName,
                    Email = isExists.Email,
                    UserName = isExists.UserName,
                    CreatedAt = isExists.CreatedAt,
                    Roles = isExists.Roles
                };

                return new UpdateUserEmailResponseDto
                {
                    userInfo = userInfoObject,
                };

            }
            catch (Exception ex)
            {
                _userLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
