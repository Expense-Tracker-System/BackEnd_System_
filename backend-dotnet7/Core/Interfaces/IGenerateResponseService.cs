using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IGenerateResponseService
    {
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        UserInfoResult GenerateUserInfoAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<LoginServiceResponseDto> GenerateOTPFor2Factor(ApplicationUser user);
    }
}
