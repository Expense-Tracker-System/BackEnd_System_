using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateUserEmailDto
    {
        [Required(ErrorMessage = "User Email is required")]
        public string? Email { get; set; }
    }
}
