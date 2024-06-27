using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Dtos.Transaction
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int Amount { get; set; }


        public string? Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;



        public TransactionStatus Status { get; set; }
    }
}
