using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetAllReminders()
        {
            var result = await _reminderService.GetAllReminders(User);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<List<Reminder>>> GetSingleReminder(int id)
        {
            var result = await _reminderService.GetSingleReminder(id);
            if (result == null)
                return NotFound("Reminder not found");
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Reminder>>> AddReminder(ReminderDto reminder)
        {
            var result = await _reminderService.AddReminder(reminder , User );
            return Ok(result);

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<List<Reminder>>> UpdateReminder(int id, ReminderDto request)
        {
            var result = await _reminderService.UpdateReminder(id, request);
            if (result == null)
                return NotFound("Reminder not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<List<Reminder>>> DeleteReminder(int id)
        {
            var result = await _reminderService.DeleteReminder(id);
            if (result == null)
                return NotFound("Reminder not found");
            return Ok(result);
        }
    }
}
