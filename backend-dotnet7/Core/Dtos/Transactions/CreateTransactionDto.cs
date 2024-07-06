using backend_dotnet7.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Dtos.Transactions
{
    public class CreateTransactionDto
    {
       
       
        public int Amount { get; set; }
       
        public string Note { get; set; }
        
        public DateTime Created { get; set; }
        public TransactionStatus Status { get; set; } //  New,Inprogress,Completed

       
    
    }
}
