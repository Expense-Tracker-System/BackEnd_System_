using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Deactivate;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Template;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public readonly IEmailService _emailService;

        public DeactivateUserAccountController(
            UserManager<ApplicationUser> userManager, 
            ApplicationDbContext applicationDbContext, 
            ILogService logService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _context = applicationDbContext;
            _loggService = logService;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("createDeactivateRequest")]
        public async Task<IActionResult> CreateDeactivateRequest([FromBody] DeactivateUserAccountDto deactivateUserAccountDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId is null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, "User not Authorized yet");
                }

                var isExist = await _userManager.FindByIdAsync(userId);

                if (isExist == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "User can't found");
                }

                if (isExist.IsDeactivateRequest)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Already has Deactivate Request");
                }

                DeactivateUserAccount deactivateUserAccount = new DeactivateUserAccount()
                {
                    UserId = isExist.Id,
                    Message = deactivateUserAccountDto.DeactivationReason,
                    Date = deactivateUserAccountDto.ReactivationDate
                };

                isExist.IsDeactivateRequest = true;
                var deactivateResponse = await _userManager.UpdateAsync(isExist);

                if (!deactivateResponse.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Deactivate request sended Failed");
                }

                await _context.DeactivateUserAccounts.AddAsync(deactivateUserAccount);
                await _context.SaveChangesAsync();
                await _loggService.SaveNewLog(isExist.UserName, "Deactivate Account");

                var admin = await _userManager.FindByNameAsync("admin");

                if(admin is null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Admin does not exist");
                }

                DeactivateRequestEmailTemplate template = new DeactivateRequestEmailTemplate();
                var htmlText = template.DeactivateRequestEmail(isExist.UserName,deactivateUserAccountDto.DeactivationReason,deactivateUserAccountDto.ReactivationDate);
                var resultOfSendEmail = await _emailService.SendEmail(htmlText, admin.Email, "Deactivate Request");

                if (resultOfSendEmail.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, "Deactivate request sended Successfully");
                }

                return StatusCode(StatusCodes.Status200OK, "Deactivate request sended Successfully, but not sended email to Admin");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getDeactivateList")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<IEnumerable<GetDeactivateListDto>>> GetDeactivateListAsync()
        {
            try
            {
                var deactivateList = await _context.DeactivateUserAccounts.ToListAsync();

                List<GetDeactivateListDto> getDeactivateList = new List<GetDeactivateListDto>();

                foreach(var deactivate in deactivateList)
                {
                    if (!deactivate.IsChecked)
                    {
                        var getDeactivate = new GetDeactivateListDto
                        {
                            Id = deactivate.Id,
                            UserId = deactivate.UserId,
                            Message = deactivate.Message,
                            Date = deactivate.Date,
                        };
                        getDeactivateList.Add(getDeactivate);
                    }
                }

                return StatusCode(StatusCodes.Status200OK, getDeactivateList);

            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("deactivateUser")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> DeactivateUserAsync([FromBody] SetDeactivateUserDto setDeactivateUserDto)
        {
            try
            {
                if (string.IsNullOrEmpty(setDeactivateUserDto.UserId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid Parameter");
                }

                var deactivatedUser = await _userManager.FindByIdAsync(setDeactivateUserDto.UserId);

                if (deactivatedUser == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Deactivate User Can't found");
                }

                deactivatedUser.LockoutEnd = setDeactivateUserDto.Date;
                deactivatedUser.IsDeactivateRequest = false;
                
                var result = await _userManager.UpdateAsync(deactivatedUser);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "User is not Deactivated");
                }

                var deactivatedAcc = await _context.DeactivateUserAccounts.FindAsync(setDeactivateUserDto.Id);

                deactivatedAcc.IsChecked = true;

                await _context.SaveChangesAsync();

                DeactivateConfirmEmailTemplate deactivateConfirmEmailTemplate = new DeactivateConfirmEmailTemplate();
                var htmlText = deactivateConfirmEmailTemplate.DeactivateConfirmEmail();
                var resultOfSendEmail = await _emailService.SendEmail(htmlText,deactivatedUser.Email,"Deactivate Account Confirmation");

                if (resultOfSendEmail.IsSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, "Successfully Deactivated");
                }

                return StatusCode(StatusCodes.Status200OK, "Successfully Deactivated, But Not Sended Email");


            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
