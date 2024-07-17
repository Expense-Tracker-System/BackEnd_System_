using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Dtos.OutMessage;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutMessagesController : Controller
    {
        private readonly IOutMessageService _outMessageService;

        public OutMessagesController(IOutMessageService outMessageService)
        {
            _outMessageService = outMessageService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateOutMessageDto createOutMessageDto)
        {
            var result = await _outMessageService.CreateMessageAsync(createOutMessageDto);

            if (result.IsSucceed)
            {
                return Ok(result.Message);
            }

            return StatusCode(result.StatusCode, result.Message);

        }
        
        [HttpGet]
        [Route("get")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<IEnumerable<GetOutMessageDto>>> GetMessages()
        {
            var result = await _outMessageService.GetMessagesAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getStartedDateEndDateOfOutMessages")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<GetStartedDateAnsEndDateMessageDto>> GetStartedDateEndDateOfOutMessages()
        {
            var result = await _outMessageService.GetStartedDateAndEndDateOfOutMessageAsync();
            return Ok(result);
        }

        [HttpPost]
        [Route("searchOutMesagesByDateRange")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<IEnumerable<GetOutMessageDto>>> SearchMesagesByDateRange([FromBody] SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var result = await _outMessageService.SearchOutMessagesByDateRangeAsync(searchMessagesByDateRangeDto);
            return Ok(result);
        }
    }
}
