using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Interfaces;
using System.Text.RegularExpressions;

namespace backend_dotnet7.Core.Services
{
    public class UserPhoneNumberService : IUserPhoneNumberService
    {
        private readonly ApplicationDbContext _context;

        public UserPhoneNumberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> IsPhoneNumberUnique(string phoneNumber)
        {
            return Task.FromResult(!_context.Users.Any(u => u.PhoneNumber == phoneNumber));
        }

        public Task<bool> PhoneNumberValidation(string phoneNumber)
        {
            Regex phoneNumberRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);

            return Task.FromResult(phoneNumberRegex.IsMatch(phoneNumber));
        }
    }
}
