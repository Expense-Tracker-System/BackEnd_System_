namespace backend_dotnet7.Core.Dtos.OutMessage
{
    public class GetOutMessageDto
    {
        public int Id { get; set; }
        public string? OutUserEmail { get; set; }
        public string? Text { get; set; }
        public bool IsChecked { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
    }
}
