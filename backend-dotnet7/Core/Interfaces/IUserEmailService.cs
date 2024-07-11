using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserEmailService
    {
        Task<bool> EmailValidation(string email);
        Task<bool> IsEmailUnique(string email);
        Task<bool> IsEmailUniqueForUpdate(string email, string userId);
    }
}
