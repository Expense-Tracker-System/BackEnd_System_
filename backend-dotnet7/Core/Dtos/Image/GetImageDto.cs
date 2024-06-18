namespace backend_dotnet7.Core.Dtos.Image
{
    public class GetImageDto
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? UserName { get; set; }
        public string UserImage { get; set; }
    }
}
