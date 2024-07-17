using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Entities;
using System.Security.Claims;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IAdminsettingService
    {
        Task<List<RegisterDto>> GetAll(ClaimsPrincipal user);
        Task<bool> UpdateUser(RegisterDto request , ClaimsPrincipal user);
    }
}
