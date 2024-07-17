using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Dtos.Message;
using System.Security.Claims;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IMessageService
    {
        Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto);
        Task<IEnumerable<GetMessageDto>> GetMessagesAsync();
        Task<IEnumerable<GetMessageDto>> GetMyMessagesAsync(ClaimsPrincipal User);
        Task<IEnumerable<GetMessageDto>> SearchMessagesByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto);
        Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfSystemMessageAsync();
        Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfMyMessageAsync(ClaimsPrincipal User);
        Task<IEnumerable<GetMessageDto>> SearchMyMessagesByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto, ClaimsPrincipal User);
    }
}
