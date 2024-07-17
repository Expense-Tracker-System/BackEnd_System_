using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Dtos.Message;
using System.Security.Claims;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ILogService
    {
        Task SaveNewLog(string UserName, string Description);
        Task<IEnumerable<GetLogDto>> GetLogsAsync();
        Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User);
        Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfSystemLogsAsync();
        Task<IEnumerable<GetLogDto>> SearchLogsByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto);
        Task<GetStartedDateAnsEndDateMessageDto> GetStartedDateAndEndDateOfMyLogsAsync(ClaimsPrincipal User);
        Task<IEnumerable<GetLogDto>> SearchMyLogsByDateRangeAsync(SearchMessagesByDateRangeDto searchMessagesByDateRangeDto, ClaimsPrincipal User);
    }
}
