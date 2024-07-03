using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(250)]
        public string Title { get; set; }


        [MaxLength(50)] 
        
        public string Icon { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
