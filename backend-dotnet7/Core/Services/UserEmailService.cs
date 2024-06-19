using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class UserEmailService : IUserEmailService
    {
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
    }
}
