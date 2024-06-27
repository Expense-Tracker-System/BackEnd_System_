using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateUserPhoneNumberDto
    {
        [Required(ErrorMessage = "User Phone number is required")]
        public string? UserPhoneNumber { get; set; }
    }
}
