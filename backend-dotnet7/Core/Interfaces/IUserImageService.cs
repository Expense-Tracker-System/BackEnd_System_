using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Image;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserImageService
    {
        Task<GetImageDto> AddUserImageAsync(AddUserImageDto addUserImageDto);
        Task<GetImageDto> UpdateUserImageAsync(UpdateUserImageDto updateUserImageDto);
        Task<GetImageDto> GetUserImageAsync();
        Task<GetImageDto> FindUserImageByUsernameAsync(string userName);
        Task DeleteUserImageAsync(string userName);
    }
}
