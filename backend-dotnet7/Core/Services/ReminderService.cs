using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend_dotnet7.Core.Services
{
    public class ReminderService : IReminderService
    {
        protected readonly ApplicationDbContext dbContext;
        public ReminderService(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public async Task<List<Reminder>> AddReminder(ReminderDto reminder , ClaimsPrincipal user)
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
    }
}
