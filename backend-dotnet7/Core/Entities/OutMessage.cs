using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class OutMessage
    {
        public int Id { get; set; }
        public string? OutUserEmail { get; set; }
        public string? Text { get; set; }
        public bool IsChecked { get; set; } = false;
        public DateTime? Created { get; set; } = DateTime.Now;
    }
}
