using backend_dotnet7.Core.Dtos.General;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Dtos.OutMessage;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IOutMessageService
    {
        Task<GeneralServiceResponseDto> CreateMessageAsync(CreateOutMessageDto createOutMessageDto);
        Task<IEnumerable<GetOutMessageDto>> GetMessagesAsync();
        Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfOutMessageAsync();
        Task<IEnumerable<GetOutMessageDto>> SearchOutMessagesByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto);
    }
}
