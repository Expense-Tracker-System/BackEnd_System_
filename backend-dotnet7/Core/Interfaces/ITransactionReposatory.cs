using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ITransactionReposatory
    {
        public List<Transaction> AllTransaction();
        public Transaction GetTransaction(int id);
    }
}
