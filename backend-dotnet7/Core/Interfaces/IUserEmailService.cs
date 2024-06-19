namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserEmailService
    {
        Task<bool> EmailValidation(string email);
    }
}
