using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class TransactionService : ITransactionReposatory
    {
        public List<Transaction> AllTransaction()
        {
            var transactions = new List<Transaction>();

            var transaction1 = new Transaction
            {
                Id = 1,
                Amount = 200,
                Note = "Food Bill",
                Created = DateTime.Now,
                Status = TransactionStatus.Completed

            };
            transactions.Add(transaction1);
            var transaction2 = new Transaction
            {
                Id = 2,
                Amount = 500,
                Note = "Water Bill",
                Created = DateTime.Now,
                Status = TransactionStatus.Completed
            };
            transactions.Add(transaction2);
            var transaction3 = new Transaction
            {
                Id = 3,
                Amount = 1000,
                Note = "Salary",
                Created = DateTime.Now,
                Status = TransactionStatus.Completed
            };
            transactions.Add(transaction3);

            return transactions;
        }

        public Transaction GetTransaction(int id)
        {
            throw new NotImplementedException();
        }
    }
}
