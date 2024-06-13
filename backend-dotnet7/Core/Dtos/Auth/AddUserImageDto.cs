using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class AddUserImageDto
    {
        [Required(ErrorMessage = "UserName is Required")] //validation part in backend
        public string Username { get; set; }

        [Required(ErrorMessage = "UserImage is Required")] //validation part in backend
        public string? UserImage { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
