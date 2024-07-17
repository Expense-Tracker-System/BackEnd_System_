using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class AddUserImageDto
    {
        [Required(ErrorMessage = "ImageFile is Required")] //validation part in backend
        public IFormFile? ImageFile { get; set; }
    }
}
