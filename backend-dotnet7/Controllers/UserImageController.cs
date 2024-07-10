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
        private readonly ILogService _logService;
        private readonly IWebHostEnvironment _environment;

        public UserImageController(IUserImageService userImageService, 
            ApplicationDbContext applicationDbContext, 
            UserManager<ApplicationUser> userManager, 
            ILogger<UserImageController> userLogger,
            ILogService logService,
            IWebHostEnvironment environment)
        {
            _userImageService = userImageService;
            _userManager = userManager;
            _userLogger = userLogger;
            _logService = logService;
            _environment = environment;
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
                    return StatusCode(StatusCodes.Status400BadRequest, "File is empty");
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
                    return StatusCode(StatusCodes.Status400BadRequest, "Add User Image is Failed");
                }

                await _logService.SaveNewLog(userExists.UserName, "Add User Image");

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

                var oldUserImage = isExist.UserImage;

                if (oldUserImage is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Username: {isExist.UserName} had not UserImage");
                }

                if (isExist.UserImage != null && updateUserImageDto.ImageFile != null)
                {
                    _userImageService.deleteUserImage(isExist.UserName, oldUserImage);
                }

                if (updateUserImageDto.ImageFile != null)
                {
                    if (updateUserImageDto.ImageFile.Length > 1 * 1024 * 1024)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
                    }

                    string[] allowedFileExtensions = new string[] { ".jpeg", ".jpg", "png" };
                    isExist.UserImage = await _userImageService.SaveUserImageAsync(isExist.UserName, updateUserImageDto.ImageFile, allowedFileExtensions);
                    //updateUserImageDto.UserImage = createdImageName;

                }

                var updateResult = await _userManager.UpdateAsync(isExist);

                if (!updateResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Update User Image is Failed");
                }

                await _logService.SaveNewLog(isExist.UserName, "Update User Image");

                return new GetImageDto
                {
                    CreatedAt = DateTime.Now,
                    UserName = isExist.UserName,
                    UserImage = isExist.UserImage
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
        public async Task<IActionResult> DeleteUserImage()
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

                if (isExist.UserImage is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User Image already not exist");
                }

                _userImageService.deleteUserImage(isExist.UserName, isExist.UserImage);

                isExist.UserImage = null;

                var updateResult = await _userManager.UpdateAsync(isExist);

                if (!updateResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Delete User Image is failed");
                }

                await _logService.SaveNewLog(isExist.UserName, "Delete User Image");

                return StatusCode(StatusCodes.Status200OK, "Delete User Image is Successfully");

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

        [HttpGet]
        [Route("CheckDirectionExist")]
        [Authorize]
        public async Task<IActionResult> CheckDirectionExists()
        {
            try
            {
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

                if (userExists.UserName is null)
                {
                    throw new ArgumentNullException("Can't create path, because userName is null");
                }

                var contentPath = _environment.ContentRootPath;
                var path = Path.Combine(contentPath, "Uploads", userExists.UserName);

                if (Directory.Exists(path))
                {
                    var filesInDirectory = Directory.GetFiles(path);
                    if(filesInDirectory.Length > 0)
                    {
                        return Ok(new { exists = true });
                    }
                    return Ok(new { exists = false });
                }
                else
                {
                    return Ok(new { exists = false });
                }
            }
            catch(Exception ex)
            {
                _userLogger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
