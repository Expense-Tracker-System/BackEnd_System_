using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Dtos.OutMessage;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class OutMessageService : IOutMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserEmailService _userEmailService;

        public OutMessageService(ApplicationDbContext context, IUserEmailService userEmailService)
        {
            _context = context;
            _userEmailService = userEmailService;
        }

        public async Task<GeneralServiceResponseDto> CreateMessageAsync(CreateOutMessageDto createOutMessageDto)
        {
            if (string.IsNullOrEmpty(createOutMessageDto.Email) || string.IsNullOrEmpty(createOutMessageDto.Text))
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Email or Text can't be null"
                };
            }

            var result = await _userEmailService.EmailValidation(createOutMessageDto.Email);

            if(result is false)
            {
                return new GeneralServiceResponseDto()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Email is Invalid"
                };
            }

            OutMessage newOutMessage = new OutMessage()
            {
                OutUserEmail = createOutMessageDto.Email,
                Text = createOutMessageDto.Text,
                IsChecked = createOutMessageDto.IsChecked,
            };

            await _context.OutMessages.AddAsync(newOutMessage);
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto()
            {
                IsSucceed = true,
                StatusCode = 200,
                Message = "Message created succesfull"
            };
        }

        public async Task<IEnumerable<GetOutMessageDto>> GetMessagesAsync()
        {
            var outMessage = await _context.OutMessages
                .Select(x => new GetOutMessageDto()
                {
                    Id = x.Id,
                    OutUserEmail = x.OutUserEmail,
                    Text = x.Text,
                    IsChecked = x.IsChecked,
                    Created = x.Created
                })
                .OrderByDescending(x => x.Created)
                .ToListAsync();

            return outMessage;
        }

        public async Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfOutMessageAsync()
        {
            if(!await _context.OutMessages.AnyAsync())
            {
                return new GetStartedDateAnsEndDateMessageDto
                {
                    IsSucceed = false,
                    StatusCode = 404,
                    Message = "Not Started the Out Message Service",
                    StartDate = null,
                    EndDate = null,
                };
            }

            var startedDate = await _context.OutMessages
                .OrderBy(om => om.Created)
                .Select(om => om.Created)
                .FirstOrDefaultAsync();

            var endDate = await _context.OutMessages
                .OrderByDescending(om => om.Created)
                .Select(om => om.Created)
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

        public async Task<IEnumerable<GetOutMessageDto>> SearchOutMessagesByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var outMessages = await _context.OutMessages
                .Where(q => q.Created >= searchMessagesByDateRangeDto.FromDate && q.Created <= searchMessagesByDateRangeDto.AdjustedToDate)
                .Select(q => new GetOutMessageDto()
                {
                    Id = q.Id,
                    OutUserEmail = q.OutUserEmail,
                    Text = q.Text,
                    IsChecked = q.IsChecked,
                    Created = q.Created,
                })
                .OrderByDescending(q => q.Created)
                .ToListAsync();

            return outMessages;
        }
    }
}
