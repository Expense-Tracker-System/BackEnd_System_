using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateUserImageDto
    {
        [Required(ErrorMessage = "UserName is Required")] //validation part in backend
        public string Username { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
