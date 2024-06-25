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
    public class UserPasswordController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserPasswordConfirmService _userPasswordConfirmService;

        public UserPasswordController(UserManager<ApplicationUser> userManager, IUserPasswordConfirmService userPasswordConfirmService)
        {
            _userManager = userManager;
            _userPasswordConfirmService = userPasswordConfirmService;
        }

        [HttpPut]
        [Route("updateUserPassword")]
        [Authorize]
        public async Task<ActionResult<GeneralServiceResponseDto>> updateUserPassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId is null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "can't find user, please contact Admin"
                    };
                }

                var isExist = await _userManager.FindByIdAsync(userId);
                if(isExist is null)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 404,
                        Message = "User deos not exist"
                    };
                }

                if (string.IsNullOrEmpty(updateUserPasswordDto.userPasswordOld))
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "Current password can't be null"
                    };
                }
                
                // check old password is correct
                var result1 = await _userManager.CheckPasswordAsync(isExist, updateUserPasswordDto.userPasswordOld);

                if(result1 is false)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "Current Password doesn't match"
                    };
                }

                if(string.IsNullOrEmpty(updateUserPasswordDto.userPasswordNew) || string.IsNullOrEmpty(updateUserPasswordDto.confirmUserPasswordNew))
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "Newly Password can't be null"
                    };
                }

                ConfirmPasswordDto confirmPasswordDto = new ConfirmPasswordDto()
                {
                    password = updateUserPasswordDto.userPasswordNew,
                    confirmPassword = updateUserPasswordDto.confirmUserPasswordNew,
                };

                // check new password confirmation
                var result2 = await _userPasswordConfirmService.userPasswordConfirm(confirmPasswordDto);

                if(result2 is false)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "New password does not match"
                    };
                }

                var result3 = await _userManager.ChangePasswordAsync(isExist, updateUserPasswordDto.userPasswordOld, updateUserPasswordDto.userPasswordNew);

                if (!result3.Succeeded)
                {
                    return new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "Update User Password Failed"
                    };
                }

                return new GeneralServiceResponseDto()
                {
                    IsSucceed = true,
                    StatusCode = 200,
                    Message = "Update User Password Successfully"
                };

            } catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
