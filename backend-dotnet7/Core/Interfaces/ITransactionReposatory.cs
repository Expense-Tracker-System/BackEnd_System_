using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ITransactionReposatory
    {
        public List<Transaction> AllTransaction(int CategoryId);

        public Transaction GetTransaction(int CategoryId, int id);

        public Transaction AddTransaction(int CategoryId, Transaction transaction);
    }
}
