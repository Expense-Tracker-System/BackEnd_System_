using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend_dotnet7.Core.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LogService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SaveNewLog(string UserName, string Description)
        {
            var newLog = new Log()
            {
                UserName = UserName,
                Description = Description,
            };

            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetLogDto>> GetLogsAsync()
        {
            var logs = await _context.Logs
                .Select(q => new GetLogDto()
                {
                    CreatedAt = q.CreatedAt,
                    UserName = q.UserName,
                    Description = q.Description
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return logs;
        }

        public async Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
        {
            var logs = await _context.Logs
                .Where(q => q.UserName == User.Identity.Name)
                .Select(q => new GetLogDto()
                {
                    CreatedAt = q.CreatedAt,
                    UserName = q.UserName,
                    Description = q.Description
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return logs;
        }

        public async Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfSystemLogsAsync()
        {
            if(!await _context.Logs.AnyAsync())
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Not Started Logs Service",
                    StartDate = null,
                    EndDate = null,
                };
            }

            var startedDate = await _context.Logs
                .OrderBy(q => q.CreatedAt)
                .Select(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            var endDate = await _context.Logs
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            if (startedDate == null || endDate == null)
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Started date or End date can't be null",
                    StartDate = startedDate,
                    EndDate = endDate,
                };
            }

            return new GetStartedDateAnsEndDateMessageDto
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "Started the Message Sercice",
                StartDate = startedDate,
                EndDate = endDate,
            };
        }

        public async Task<IEnumerable<GetLogDto>> SearchLogsByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var logs = await _context.Logs
                .Where(q => q.CreatedAt >= searchMessagesByDateRangeDto.FromDate && q.CreatedAt <= searchMessagesByDateRangeDto.AdjustedToDate)
                .Select(q => new GetLogDto()
                {
                    CreatedAt = q.CreatedAt,
                    UserName = q.UserName,
                    Description = q.Description,
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return logs;
        }

        public async Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfMyLogsAsync(ClaimsPrincipal User)
        {
            if (User.Identity.Name is null)
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "User Name can't be null",
                    StartDate = null,
                    EndDate = null,
                };
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 401,
                    Message = "User is not Authorized",
                    StartDate = null,
                    EndDate = null,
                };
            }

            if (!await _context.Messages.AnyAsync())
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Not Started the Logs Service",
                    StartDate = null,
                    EndDate = null,
                };
            }

            var startedDate = await _context.Logs
                .Where(q => q.UserName == user.UserName)
                .OrderBy(q => q.CreatedAt)
                .Select(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            var endDate = await _context.Logs
                .Where(q => q.UserName == user.UserName)
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            if (startedDate == null || endDate == null)
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Started date or End date can't be null",
                    StartDate = startedDate,
                    EndDate = endDate,
                };
            }

            return new GetStartedDateAnsEndDateMessageDto
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "Started the Logs Sercice",
                StartDate = startedDate,
                EndDate = endDate,
            };
        }

        public async Task<IEnumerable<GetLogDto>> SearchMyLogsByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto, ClaimsPrincipal User)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var userLogs = await _context.Logs
                .Where(q => q.UserName == user.UserName && q.CreatedAt >= searchMessagesByDateRangeDto.FromDate && q.CreatedAt <= searchMessagesByDateRangeDto.AdjustedToDate)
                .Select(q => new GetLogDto()
                {
                    CreatedAt = q.CreatedAt,
                    UserName = q.UserName,
                    Description = q.Description,
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return userLogs;
        }
    }
}
