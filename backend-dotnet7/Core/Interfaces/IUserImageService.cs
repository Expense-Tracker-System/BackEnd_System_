using backend_dotnet7.Core.Dtos.Auth;
using backend_dotnet7.Core.Dtos.Image;
using Microsoft.AspNetCore.Http;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IUserImageService
    {
        Task<string> SaveUserImageAsync(IFormFile imageFile, string[] allowedFileExtensions);
        void deleteUserImage(string imageNameWithExtension);
    }
}
