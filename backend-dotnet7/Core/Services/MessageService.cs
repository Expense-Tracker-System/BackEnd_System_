using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend_dotnet7.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(ApplicationDbContext context,ILogService logService,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logService = logService;
            _userManager = userManager;
        }

        public async Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if(user is null)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 401,
                    Message = "User is not Authorized"
                };
            }

            if (user.UserName == createMessageDto.ReceiverUserName)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Sender and Receiver can not be same"
                };

            var isReceiverUserNameValid = _userManager.Users.Any(q => q.UserName == createMessageDto.ReceiverUserName);
            if(!isReceiverUserNameValid)
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Receiver UserName is not valid"
                };

            Message newMessage = new Message()
            {
                SenderUserName = User.Identity.Name,
                ReceiverUserName = createMessageDto.ReceiverUserName,
                Text = createMessageDto.Text,
                IsChecked = false,
                UserId = user.Id,
            };

            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            await _logService.SaveNewLog(User.Identity.Name,"Send Message");

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 201,
                Message = "Message Save Succesfully"
            };
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessagesAsync()
        {
            var messages = await _context.Messages
                .Select(q => new GetMessageDto()
                {
                    Id = q.Id,
                    SenderUserName = q.SenderUserName,
                    ReceiverUserName = q.ReceiverUserName,
                    Text = q.Text,
                    CreatedAt = q.CreatedAt
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User)
        {
            var messages = await _context.Messages
                .Where(q => q.SenderUserName == User.Identity.Name || q.ReceiverUserName == User.Identity.Name)
                .Select(q => new GetMessageDto()
                {
                    Id = q.Id,
                    SenderUserName = q.SenderUserName,
                    ReceiverUserName = q.ReceiverUserName,
                    Text = q.Text,
                    CreatedAt = q.CreatedAt
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return messages;
        }

        public async Task<IEnumerable<GetMessageDto>> SearchMessagesByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var messages = await _context.Messages
                .Where(q => q.CreatedAt >= searchMessagesByDateRangeDto.FromDate && q.CreatedAt <= searchMessagesByDateRangeDto.AdjustedToDate)
                .Select(q => new GetMessageDto()
                {
                    Id = q.Id,
                    SenderUserName = q.SenderUserName,
                    ReceiverUserName = q.ReceiverUserName,
                    Text = q.Text,
                    CreatedAt = q.CreatedAt
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return messages;
        }

        public async Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfSystemMessageAsync()
        {
            if(!await _context.Messages.AnyAsync())
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Not Started the Message Service",
                    StartDate = null,
                    EndDate = null,
                };
            }

            var startedDate = await _context.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => m.CreatedAt)
                .FirstOrDefaultAsync();

            var endDate = await _context.Messages
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => m.CreatedAt)
                .FirstOrDefaultAsync();

            if(startedDate == null || endDate == null)
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Started date or End date can't be null",
                    StartDate= startedDate,
                    EndDate= endDate,
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

        public async Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfMyMessageAsync(ClaimsPrincipal User)
        {
            if(User.Identity.Name is null)
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

            if(user == null)
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
                    Message = "Not Started the Message Service",
                    StartDate = null,
                    EndDate = null,
                };
            }

            var startedDate = await _context.Messages
                .Where(q => q.UserId == user.Id)
                .OrderBy(q => q.CreatedAt)
                .Select(q => q.CreatedAt)
                .FirstOrDefaultAsync();

            var endDate = await _context.Messages
                .Where(q => q.UserId == user.Id)
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

        public async Task<IEnumerable<GetMessageDto>> SearchMyMessagesByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto, ClaimsPrincipal User)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var messages = await _context.Messages
                .Where(q => q.UserId == user.Id && q.CreatedAt >= searchMessagesByDateRangeDto.FromDate && q.CreatedAt <= searchMessagesByDateRangeDto.AdjustedToDate)
                .Select(q => new GetMessageDto()
                {
                    Id = q.Id,
                    SenderUserName = q.SenderUserName,
                    ReceiverUserName = q.ReceiverUserName,
                    Text = q.Text,
                    CreatedAt = q.CreatedAt
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return messages;
        }
    }
}
