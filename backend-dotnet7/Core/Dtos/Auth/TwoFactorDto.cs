namespace backend_dotnet7.Core.Dtos.Auth
{
    public class TwoFactorDto
    {
        public string? userName { get; set; }
        public string? provider { get; set; }
        public string? token { get; set; }
    }
}
