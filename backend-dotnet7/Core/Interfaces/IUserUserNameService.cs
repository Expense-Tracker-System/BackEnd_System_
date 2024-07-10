using System.Security.Claims;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserUserNameService
    {
        Task<bool> userUserNameConfirm(string currentUserName, ClaimsPrincipal User);
        Task<bool> userUserNameAlreadyExist(string newUserName);
    }
}
