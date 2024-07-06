using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Controllers
{
    public class UserPhoneNumberController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserPhoneNumberService _userPhoneNumberService;

        public UserPhoneNumberController(UserManager<ApplicationUser> userManager, IUserPhoneNumberService userPhoneNumberService)
        {
            _userManager = userManager;
            _userPhoneNumberService = userPhoneNumberService;
        }

        [HttpPut]
        [Route("updateUserPhoneNumber")]
        [Authorize]
        public async Task<ActionResult<GeneralServiceResponseDto>> updateUserPhoneNumber([FromBody] UpdateUserPhoneNumberDto updateUserPhoneNumberDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User is not Unauthorized");
                }

                var isExist = await _userManager.FindByIdAsync(userId);

                if (isExist is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Can't find user");
                }

                if (string.IsNullOrEmpty(updateUserPhoneNumberDto.UserPhoneNumber))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Phone Number can't be null");
                }

                var isValid = await _userPhoneNumberService.PhoneNumberValidation(updateUserPhoneNumberDto.UserPhoneNumber);

                if (isValid is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Phone Number is Invalid");
                }

                var isUnique = await _userPhoneNumberService.IsPhoneNumberUnique(updateUserPhoneNumberDto.UserPhoneNumber);

                if (isUnique is false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User Phone Number is not Unique");
                }

                var token = await _userManager.GenerateChangePhoneNumberTokenAsync(isExist, updateUserPhoneNumberDto.UserPhoneNumber);

                var result = await _userManager.ChangePhoneNumberAsync(isExist, updateUserPhoneNumberDto.UserPhoneNumber,token);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Update User Phone Number is Failed");
                }

                return new GeneralServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Successfully Update Phone Number"
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
