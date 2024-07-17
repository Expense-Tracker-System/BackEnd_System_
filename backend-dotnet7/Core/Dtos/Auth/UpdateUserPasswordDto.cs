using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateUserPasswordDto
    {
        [Required(ErrorMessage = "Old UserPassword is required")]
        public string? userPasswordOld { get; set; }

        [Required(ErrorMessage = "New UserPassword is required")]
        public string? userPasswordNew { get; set; }

        [Required(ErrorMessage = "Confirmation is required")]
        public string? confirmUserPasswordNew { get; set; }
    }
}
