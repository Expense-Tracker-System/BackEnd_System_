using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Dtos.OutMessage;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class OutMessageService : IOutMessageService
    {
        private readonly ApplicationDbContext _context;

        public OutMessageService(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
