using backend_dotnet7.Core.Interfaces;
using System.Security.Claims;

namespace backend_dotnet7.Core.Services
{
    public class UserUserNameService : IUserUserNameService
    {
        private readonly IAuthService _authService;

        public UserUserNameService(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<bool> userUserNameAlreadyExist(string newUserName)
        {
            if (string.IsNullOrEmpty(newUserName))
            {
                return false;
            }

            var userNameList = await _authService.GetUsernameListAsync();
            if (userNameList != null)
            {
                if (userNameList.Contains(newUserName))
                {
                    return false;
                }
            }

            return true;

        }

        public Task<bool> userUserNameConfirm(string currentUserName, ClaimsPrincipal User)
        {
            if (!string.IsNullOrEmpty(currentUserName))
            {
                if(currentUserName == User.Identity.Name)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
    }
}
