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
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserEmailService _userEmailService;
        private readonly IUserPhoneNumberService _userPhoneNumberService;
        private readonly IGenerateResponseService _generateResponseService;
        private readonly ILogService _logService;

        public UserProfileController(UserManager<ApplicationUser> userManager,
            IUserEmailService userEmailService,
            IUserPhoneNumberService userPhoneNumberService,
            IGenerateResponseService generateResponseService,
            ILogService logService)
        {
            _userManager = userManager;
            _userEmailService = userEmailService;
            _userPhoneNumberService = userPhoneNumberService;
            _generateResponseService = generateResponseService;
            _logService = logService;
        }

        [HttpPut]
        [Route("updateUserProfile")]
        [Authorize]
        public async Task<ActionResult<LoginServiceResponseDto>> updateUserProfileAsync([FromBody] UpdateUserProfileDto updateUserProfileDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not Authorized");
                }

                var isExist = await _userManager.FindByIdAsync(userId);
                if (isExist is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User is not found");
                }

                if(string.IsNullOrEmpty(updateUserProfileDto.UserFirstName) || string.IsNullOrEmpty(updateUserProfileDto.UserLastName) ||
                    string.IsNullOrEmpty(updateUserProfileDto.UserEmail) || string.IsNullOrEmpty(updateUserProfileDto.UserPhoneNumber))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "First Name or Last Name or Email or Phone Number can't be null");
                }

                var isValidEmail =  await _userEmailService.EmailValidation(updateUserProfileDto.UserEmail);
                if (isValidEmail is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Email is Invalid");
                }

                var isUniqueEmail =  await _userEmailService.IsEmailUniqueForUpdate(updateUserProfileDto.UserEmail, userId);
                if(isUniqueEmail is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Email is not Unique");
                }

                var isValidPhoneNumber = await _userPhoneNumberService.PhoneNumberValidation(updateUserProfileDto.UserPhoneNumber);
                if(isValidPhoneNumber is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Phone Number is Invalid");
                }

                var isUniquePhoneNumber = await _userPhoneNumberService.IsPhoneNumberUniqueForUpdate(updateUserProfileDto.UserPhoneNumber, userId);
                if(isUniquePhoneNumber is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Phone Number is not Unique");
                }

                isExist.FirstName = updateUserProfileDto.UserFirstName;
                isExist.LastName = updateUserProfileDto.UserLastName;
                isExist.Email = updateUserProfileDto.UserEmail;
                isExist.NormalizedEmail = _userManager.NormalizeEmail(updateUserProfileDto.UserEmail);
                isExist.PhoneNumber = updateUserProfileDto.UserPhoneNumber;

                var updateResult = await _userManager.UpdateAsync(isExist);
                if (!updateResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Update User Profile is failed");
                }

                var newToken = await _generateResponseService.GenerateJwtTokenAsync(isExist);

                var roles = await _userManager.GetRolesAsync(isExist);
                var userInfo = _generateResponseService.GenerateUserInfoAsync(isExist, roles);

                await _logService.SaveNewLog(isExist.UserName, "Update User Profile");

                return new LoginServiceResponseDto
                {
                    NewToken = newToken,
                    userInfo = userInfo,
                };

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
