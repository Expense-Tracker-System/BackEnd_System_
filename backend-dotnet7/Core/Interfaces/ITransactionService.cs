using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> AddTransaction(TransactionDto transaction, string userName);
        IEnumerable<TransactionDto> GetTransactions(string userName);
        Task<bool> DeleteTransaction(int id, string userName);
        Task<TransactionDto> UpdateTransaction(int id, TransactionDto transaction, string userName);
    }
}
