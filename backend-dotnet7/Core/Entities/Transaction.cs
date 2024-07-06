using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [MaxLength(100)]
        public string Note { get; set; }
        [Required]
        public DateTime Created {  get; set; }
        public TransactionStatus Status { get; set; } //  New,Inprogress,Completed

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
