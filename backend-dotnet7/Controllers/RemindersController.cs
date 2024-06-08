using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend_dotnet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        public RemindersController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reminder>>> GetAllReminders()
        {
            var result = await _reminderService.GetAllReminders();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Reminder>>> GetSingleReminder(int id)
        {
            var result = await _reminderService.GetSingleReminder(id);
            if (result == null)
                return NotFound("Reminder not found");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<Reminder>>> AddReminder(ReminderDto reminder)
        {
            var result = await _reminderService.AddReminder(reminder);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Reminder>>> UpdateReminder(int id, ReminderDto request)
        {
            var result = await _reminderService.UpdateReminder(id, request);
            if (result == null)
                return NotFound("Reminder not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Reminder>>> DeleteReminder(int id)
        {
            var result = await _reminderService.DeleteReminder(id);
            if (result == null)
                return NotFound("Reminder not found");
            return Ok(result);
        }
    }
}
