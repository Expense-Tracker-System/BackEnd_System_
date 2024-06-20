namespace backend_dotnet7.Core.Dtos.Auth
{
    public class ConfirmPasswordDto
    {
        public string? password { get; set; }
        public string? confirmPassword { get; set; }
    }
}
