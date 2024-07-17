using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetLogs()
        {
            var logs = await _logService.GetLogsAsync();
            return Ok(logs);
        }

        [HttpGet]
        [Route("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetLogDto>>> GetMyLogs()
        {
            var logs = await _logService.GetMyLogsAsync(User);
            return Ok(logs);
        }

        [HttpGet]
        [Route("getStartedDateAndEndDateOfSystemLogs")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<GetStartedDateAnsEndDateMessageDto>> GetStartedDateAndEndDateOfSystemLogs()
        {
            var result = await _logService.GetStartedDateAndEndDateOfSystemLogsAsync();
            return Ok(result);
        }
            
        [HttpPost]
        [Route("searchLogsByDateRange")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<IEnumerable<GetLogDto>>> SearchLogsByDateRange([FromBody] SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var result = await _logService.SearchLogsByDateRangeAsync(searchMessagesByDateRangeDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("getStartedDateAndEndDateOfMyLogs")]
        public async Task<ActionResult<GetStartedDateAnsEndDateMessageDto>> GetStartedDateAndEndDateOfMyLogs()
        {
            var result = await _logService.GetStartedDateAndEndDateOfMyLogsAsync(User);
            return Ok(result);
        }

        [HttpPost]
        [Route("searchMyLogsByDateRange")]
        public async Task<ActionResult<IEnumerable<GetLogDto>>> SearchMyLogsByDateRange([FromBody] SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var result = await _logService.SearchMyLogsByDateRangeAsync(searchMessagesByDateRangeDto, User);
            return Ok(result);
        }
    }
}
