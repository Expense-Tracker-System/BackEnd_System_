using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Image;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Controllers
{
    [Route("api/UserImage")]
    [ApiController]
    public class UserImageController : ControllerBase
    {
        private readonly IUserImageService _userImageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserImageController> _userLogger;


        public UserImageController(IUserImageService userImageService, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, ILogger<UserImageController> userLogger)
        {
            _userImageService = userImageService;
            _userManager = userManager;
            _userLogger = userLogger;
        }

        [HttpPost]
        [Route("AddUserImage")]
        [Authorize]
        public async Task<IActionResult> AddUserImage([FromForm] AddUserImageDto addUserImageDto)
        {
            try
            {
                if (addUserImageDto.ImageFile is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "File is empty");
                }

                if (addUserImageDto.ImageFile.Length > 1 * 1024 * 1024)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
                }

                string[] allowedFileExtensions = new string[] { ".jpeg", ".jpg", "png" };

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User does not exist.");
                }
                var userExists = await _userManager.FindByIdAsync(userId);
                if (userExists is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User does not exist.");
                }
                

                if(userExists.UserName is null)
                {
                    throw new ArgumentNullException("Can't create path, because userName is null");
                }

                var createdImageName = await _userImageService.SaveUserImageAsync(userExists.UserName, addUserImageDto.ImageFile, allowedFileExtensions);

                userExists.UserImage = createdImageName;

                var updateResult = await _userManager.UpdateAsync(userExists);

                if (!updateResult.Succeeded)
                {
                    return null;
                }

                return Ok(updateResult);
            }
            catch (Exception ex)
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
                if (isExistsUser is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {updateUserImageDto.Username} does not found");
                }

                var oldUserImage = isExistsUser.UserImage;

                if (oldUserImage is null)
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
            catch (Exception ex)
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
                if (isExistsUser is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {userName} does not found");
                }

                if (isExistsUser.UserImage is not null)
                {
                    _userImageService.deleteUserImage(userName, isExistsUser.UserImage);
                }

                isExistsUser.UserImage = null;

                var updateResult = await _userManager.UpdateAsync(isExistsUser);

                if (!updateResult.Succeeded)
                {
                    return null;
                }

                return NoContent();

            } catch (Exception ex)
            {
                _userLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUserImage")]
        [Authorize]
        public async Task<ActionResult<GetImageDto?>> GetUserImageByUserName()
        {
            if (User?.Identity?.Name == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "User is not authenticated.");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if(user is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"userName : {User.Identity.Name} Not founded");
            }

            if(user.UserImage is null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"userName : {user.UserName} with userImage Not founded");
            }

            return new GetImageDto
            {
                CreatedAt = DateTime.Now,
                UserName = user.UserName,
                UserImage = user.UserImage
            };
        
        }
    }
}
