using backend_dotnet7.Core.Constants;
using backend_dotnet7.Core.Dtos.Message;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        //Route -> Create a new message to send to another user
        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewMessage([FromBody] CreateMessageDto createMessageDto)
        {
            var result = await _messageService.CreateNewMessageAsync(User, createMessageDto);
            if (result.IsSucceed)
                return Ok(result.Message);

            return StatusCode(result.StatusCode, result.Message);
        }


        //Route -> Get All Messages for current user, Either as Sender or as Receiver
        [HttpGet]
        [Route("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMyMessages()
        {
            var messages = await _messageService.GetMyMessagesAsync(User);
            return Ok(messages);
        }


        //Route -> Get All Messages With Admine Access
        [HttpGet]
        [Route("getAllMessage")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMessages()
        {
            var messages = await _messageService.GetMessagesAsync();
            return Ok(messages);
        }

        [HttpGet]
        [Route("getStartedDateAndEndDateOfSystemMessage")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<ActionResult<GetStartedDateAnsEndDateMessageDto>> GetStartedDateAndEndDateOfSystemMessage()
        {
            var startedDate = await _messageService.GetStartedDateAndEndDateOfSystemMessageAsync();
            return Ok(startedDate);
        }

        [HttpPost]
        [Route("searchMesagesByDateRange")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> SearchMessagesByDateRange([FromBody] SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var result = await _messageService.SearchMessagesByDateRangeAsync(searchMessagesByDateRangeDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("getStartedDateAndEndDateOfMyMessage")]
        [Authorize]
        public async Task<ActionResult<GetStartedDateAnsEndDateMessageDto>> GetStartedDateAndEndDateOfMyMessage()
        {
            var result = await _messageService.GetStartedDateAndEndDateOfMyMessageAsync(User);
            return Ok(result);
        }

        [HttpPost]
        [Route("searchMyMesagesByDateRange")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> SearchMyMesagesByDateRange([FromBody] SearchMessagesByDateRangeDto searchMessagesByDateRangeDto)
        {
            var result = await _messageService.SearchMyMessagesByDateRangeAsync(searchMessagesByDateRangeDto, User);
            return Ok(result);
        }
    }
}
