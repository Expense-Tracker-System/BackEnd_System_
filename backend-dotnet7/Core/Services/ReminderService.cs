using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using backend_dotnet7.Core.Template;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace backend_dotnet7.Core.Services
{
    public class ReminderService : IReminderService
    {
        protected readonly ApplicationDbContext dbContext;
        private readonly IEmailService _emailService;

        public ReminderService(ApplicationDbContext context, IEmailService emailService)
        {
            dbContext = context;
            _emailService = emailService;
        }
        public async Task<List<Reminder>> AddReminder(ReminderDto reminder, ClaimsPrincipal user)
        {
            var newrem = new Reminder
            {
                ReminderName = reminder.ReminderName,
                ReminderAmount = reminder.ReminderAmount,
                ReminderDescription = reminder.ReminderDescription,
                ReminderstartDate = reminder.ReminderstartDate,
                ReminderendDate = reminder.ReminderendDate,
                UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            };
            dbContext.Reminders.Add(newrem);
            await dbContext.SaveChangesAsync();
            try
            {
                var email = await dbContext.Users.Where(u => u.Id == newrem.UserId).Select(u => u.Email).FirstOrDefaultAsync();
              var text = new EmailTemplate();
                var htmltext = text.reminderset(newrem.ReminderName, newrem.ReminderstartDate, newrem.ReminderAmount, newrem.ReminderDescription);
                await _emailService.SendEmail(htmltext, email, "Reminder set");
            }
            catch
            {

            }

            return await dbContext.Reminders.ToListAsync();

        }

        public async Task<List<Reminder>?> DeleteReminder(int id)
        {
            var rem = await dbContext.Reminders.FindAsync(id);

            if (rem is null)
                return null;

            dbContext.Reminders.Remove(rem);
            await dbContext.SaveChangesAsync();
            return await dbContext.Reminders.ToListAsync();
        }

        public async Task<List<Reminder>> GetAllReminders(ClaimsPrincipal User)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var reminders = await dbContext.Reminders
                .Where(q => q.UserId == userId)
                .Select(q => new Reminder()
                {
                    ReminderId = q.ReminderId,
                    ReminderName = q.ReminderName,
                    ReminderAmount = q.ReminderAmount,
                    ReminderDescription = q.ReminderDescription,
                    ReminderstartDate = q.ReminderstartDate,
                    ReminderendDate = q.ReminderendDate
                })
                .ToListAsync();

            return reminders;
        }

        public async Task<Reminder?> GetSingleReminder(int id)
        {
            var rem = await dbContext.Reminders.FindAsync(id);

            if (rem is null) return null;

            return rem;
        }

        public async Task<List<Reminder>?> UpdateReminder(int id, ReminderDto request)
        {
            var rem = await dbContext.Reminders.FindAsync(id);

            if (rem is null) return null;

            rem.ReminderName = request.ReminderName;
            rem.ReminderAmount = request.ReminderAmount;
            rem.ReminderAmount = request.ReminderAmount;
            await dbContext.SaveChangesAsync();

            return await dbContext.Reminders.ToListAsync();
        }
        public async Task ReminderMail()
        {
            var reminders = await dbContext.Reminders.ToListAsync();
            foreach (var item in reminders)
            {
                if(DateOnly.FromDateTime(item.ReminderstartDate) == DateOnly.FromDateTime(DateTime.Today))
                {
                    try
                    {
                        var email = await dbContext.Users.Where(u => u.Id == item.UserId).Select(u => u.Email).FirstOrDefaultAsync();
                        var text = new EmailTemplate();
                        var htmltext = text.remindertoday(item.ReminderName, item.ReminderAmount, item.ReminderDescription);
                        await _emailService.SendEmail(htmltext, email, "Reminder Today");
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}  
    