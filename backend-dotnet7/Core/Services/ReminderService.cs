using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class ReminderService : IReminderService
    {
        protected readonly ApplicationDbContext datacontext;
        public ReminderService(ApplicationDbContext context) 
        {
            this.datacontext = context;
        }
        public async Task<List<Reminder>> AddReminder(ReminderDto reminder)
        {
            var newrem = new Reminder 
            {
                ReminderName = reminder.ReminderName,
                RAmount = reminder.RAmount,
                RDescription = reminder.RDescription,
                ReminderDate = DateOnly.Parse(reminder.ReminderDate)
            };
            datacontext.Reminders.Add(newrem);
            await datacontext.SaveChangesAsync();
            return await datacontext.Reminders.ToListAsync();
        }

        public async Task<List<Reminder>?> DeleteReminder(int id)
        {
            var rem = await datacontext.Reminders.FindAsync(id);

            if (rem is null)
                return null;

            datacontext.Reminders.Remove(rem);
            await datacontext.SaveChangesAsync();
            return await datacontext.Reminders.ToListAsync();
        }

        public async Task<List<Reminder>> GetAllReminders()
        {
            var reminders = await datacontext.Reminders.ToListAsync();
            return reminders;
        }

        public async Task<Reminder?> GetSingleReminder(int id)
        {
            var rem = await datacontext.Reminders.FindAsync(id);

            if (rem is null) return null;

            return rem;
        }

        public async Task<List<Reminder>?> UpdateReminder(int id, ReminderDto request)
        {
            var rem = await datacontext.Reminders.FindAsync(id);

            if (rem is null) return null;

            rem.ReminderName = request.ReminderName;
            rem.RAmount = request.RAmount;
            rem.RDescription = request.RDescription;
            await datacontext.SaveChangesAsync();

            return await datacontext.Reminders.ToListAsync();
        }
    }
}
