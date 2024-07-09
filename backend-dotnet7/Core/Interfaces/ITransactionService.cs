using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Dtos;
using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace backend_dotnet7.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionDto> AddTransaction(string userName, TransactionDto transaction);
        IEnumerable<TransactionDto> GetTransactions(string userName);
        Task<bool> DeleteTransaction(int id, string userName);
        Task<TransactionEntity> UpdateTransaction( TransactionDto transaction,string userName);
    }
}
