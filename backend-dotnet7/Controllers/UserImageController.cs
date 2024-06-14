using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Image;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/UserImage")]
    [ApiController]
    public class UserImageController : Controller
    {
        private readonly IUserImageService _userImageService;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserImageController> _userLogger;


        public UserImageController(IUserImageService userImageService, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, ILogger<UserImageController> userLogger)
        {
            _userImageService = userImageService;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _userLogger = userLogger;
        }

        [HttpPost]
        [Route("AddUserImage")]
        [Authorize]
        public async Task<ActionResult<GetImageDto>> AddUserImage([FromForm] AddUserImageDto addUserImageDto)
        {
            try
            {
                if(addUserImageDto.ImageFile.Length > 1 * 1024 * 1024)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
                }

                string[] allowedFileExtensions = new string[] { ".jpeg", ".jpg", "png" };
                var createdImageName = await _userImageService.SaveUserImageAsync(addUserImageDto.Username, addUserImageDto.ImageFile, allowedFileExtensions);

                var isExistsUser = await _userManager.FindByNameAsync(addUserImageDto.Username);

                if(isExistsUser is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {addUserImageDto.Username} does not found");
                }

                isExistsUser.UserImage = createdImageName;

                var updateResult = await _userManager.UpdateAsync(isExistsUser);

                if (!updateResult.Succeeded)
                {
                    return null;
                }

                return new GetImageDto
                {
                    CreatedAt = DateTime.Now,
                    UserName = addUserImageDto.Username,
                    UserImage = createdImageName
                };
            }
            catch(Exception ex)
            {
                _userLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUserImage")]
        [Authorize]
        public async Task<ActionResult<GetImageDto>> UpdateUserImage([FromForm] UpdateUserImageDto updateUserImageDto)
        {
            try
            {
                var isExistsUser = await _userManager.FindByNameAsync(updateUserImageDto.Username);
                if(isExistsUser is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {updateUserImageDto.Username} does not found");
                }

                var oldUserImage = isExistsUser.UserImage;

                if(oldUserImage is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {updateUserImageDto.Username} had not UserImage");
                }

                if (isExistsUser.UserImage != null && updateUserImageDto.ImageFile != null)
                {
                    _userImageService.deleteUserImage(updateUserImageDto.Username, oldUserImage);
                }

                if (updateUserImageDto.ImageFile != null)
                {
                    if (updateUserImageDto.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
                    }

                    string[] allowedFileExtensions = new string[] { ".jpeg", ".jpg", "png" };
                    var createdImageName = await _userImageService.SaveUserImageAsync(updateUserImageDto.Username, updateUserImageDto.ImageFile, allowedFileExtensions);
                    updateUserImageDto.UserImage = createdImageName;

                }

                // -----
                isExistsUser.UserImage = updateUserImageDto.UserImage;

                var updateResult = await _userManager.UpdateAsync(isExistsUser);

                if (!updateResult.Succeeded)
                {
                    return null;
                }

                return new GetImageDto
                {
                    CreatedAt = DateTime.Now,
                    UserName = updateUserImageDto.Username,
                    UserImage = updateUserImageDto.UserImage
                };

            }
            catch(Exception ex)
            {
                _userLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteUserImage")]
        [Authorize]
        public async Task<IActionResult> DeleteUserImage(string userName)
        {
            try
            {
                var isExistsUser = await _userManager.FindByNameAsync(userName);
                if(isExistsUser is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {userName} does not found");
                }

                if(isExistsUser.UserImage is not null)
                {
                    _userImageService.deleteUserImage(userName, isExistsUser.UserImage);
                }

                isExistsUser.UserImage = null;

                var updateResult = await _userManager.UpdateAsync(isExistsUser);

                if(!updateResult.Succeeded)
                {
                    return null;
                }

                return NoContent();

            }catch(Exception ex)
            {
                _userLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
