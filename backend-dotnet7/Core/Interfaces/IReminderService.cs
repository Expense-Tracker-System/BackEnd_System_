using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Reminder;
using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IReminderService
    {
        Task<List<Reminder>> GetAllReminders(ClaimsPrincipal user);
        Task<Reminder?> GetSingleReminder(int id);
        Task<List<Reminder>> AddReminder(ReminderDto reminder , ClaimsPrincipal User);
        Task<List<Reminder>?> UpdateReminder(int id, ReminderDto request);
        Task<List<Reminder>?> DeleteReminder(int id);
        Task ReminderMail();
    }
}
