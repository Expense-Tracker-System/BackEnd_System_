using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class UserPasswordConfirmService : IUserPasswordConfirmService
    {
        public Task<bool> userPasswordConfirm(ConfirmPasswordDto confirmPasswordDto)
        {
            if(confirmPasswordDto.password is null || confirmPasswordDto.confirmPassword is null)
            {
                return Task.FromResult(false);
            }

            if(confirmPasswordDto.password == confirmPasswordDto.confirmPassword)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
