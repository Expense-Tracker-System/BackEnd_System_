namespace backend_dotnet7.Core.Dtos.Deactivate
{
    public class SetDeactivateUserDto
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
