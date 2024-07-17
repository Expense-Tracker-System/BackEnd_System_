using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace backend_dotnet7.Core.Services
{
    public class UserEmailService : IUserEmailService
    {
        private readonly ApplicationDbContext _context;

        public UserEmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> EmailValidation(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return Task.FromResult(true);

            } catch(FormatException)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> IsEmailUnique(string email)
        {
            return Task.FromResult(!_context.Users.Any(u => u.Email == email));
        }

        public Task<bool> IsEmailUniqueForUpdate(string email, string userId)
        {
            return Task.FromResult(!_context.Users.Any(u => u.Email == email && u.Id != userId));
        }
    }
}
