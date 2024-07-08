namespace backend_dotnet7.Core.Dtos.Auth
{
    public class UpdateUserProfileDto
    {
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set;}
        public string? UserEmail { get; set;}
        public string? UserPhoneNumber { get; set;}
    }
}
