namespace backend_dotnet7.Core.Dtos.Auth
{
    public class DeactivateUserAccountDto
    {
        public string? DeactivationReason { get; set; }
        public DateTime ReactivationDate { get; set; }
    }
}
