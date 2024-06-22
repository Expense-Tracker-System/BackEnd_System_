using backend_dotnet7.Core.Dtos.Auth;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserPasswordConfirmService
    {
        Task<bool> userPasswordConfirm(ConfirmPasswordDto confirmPasswordDto);
    }
}
