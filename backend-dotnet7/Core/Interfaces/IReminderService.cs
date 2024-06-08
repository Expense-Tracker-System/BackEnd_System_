using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IReminderService
    {
        Task<List<Reminder>> GetAllReminders();
        Task<Reminder?> GetSingleReminder(int id);
        Task<List<Reminder>> AddReminder(ReminderDto reminder);
        Task<List<Reminder>?> UpdateReminder(int id, ReminderDto request);
        Task<List<Reminder>?> DeleteReminder(int id);
    }
}
