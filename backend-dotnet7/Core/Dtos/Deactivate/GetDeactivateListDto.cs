namespace backend_dotnet7.Core.Dtos.Deactivate
{
    public class GetDeactivateListDto
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}
