using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionEntities> AddTransaction(TransactionEntities transaction);
        IEnumerable<TransactionEntities> GetTransactions();
        Task<bool> DeleteTransaction(int id);
        Task<TransactionEntities> UpdateTransaction(int id, TransactionEntities transaction);
    }
}
