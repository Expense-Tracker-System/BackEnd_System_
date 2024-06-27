namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserPhoneNumberService
    {
        Task<bool> PhoneNumberValidation(string phoneNumber);
        Task<bool> IsPhoneNumberUnique(string phoneNumber);
    }
}
