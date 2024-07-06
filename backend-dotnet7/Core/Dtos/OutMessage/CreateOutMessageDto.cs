namespace backend_dotnet7.Core.Dtos.OutMessage
{
    public class CreateOutMessageDto
    {
        public string? Email { get; set; }
        public string? Text { get; set; }
        public bool IsChecked { get; set; }
    }
}
