using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos;
using backend_dotnet7.Core.Dtos.Log;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionDto> AddTransaction(string userName, TransactionDto transaction)
        {
            var entity = new TransactionEntity
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                userName = userName,
            };

            _context.TransactionEntities.Add(entity);
            await _context.SaveChangesAsync();

            return transaction;
        }

       
       
        

        
        public IEnumerable<TransactionDto> GetTransactions(string userName)
            {
            var transactions = _context.TransactionEntities.Where(t => t.userName == userName).Select(t => new TransactionDto
            {
                    Id = t.Id,
                    Amount = t.Amount,
                    Description = t.Description,
                     // Assuming CreatedAt as Date
                })
                .ToList();

            return transactions;
        }

        public async Task<bool> DeleteTransaction(int id, string userName)
        {
            var entity = new TransactionEntity
        {
                Id = id,
              
                userName =userName, // Assign the UserName from DTO
            };
            _context.TransactionEntities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionEntity> UpdateTransaction( TransactionDto transaction, string userName)
        {
            var existingTransaction = await _context.
                TransactionEntities.FirstOrDefaultAsync(t => t.Id == transaction.Id && t.userName == userName);

            if (existingTransaction == null)
                return null;

            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Description = transaction.Description;

            _context.TransactionEntities.Update(existingTransaction);
            await _context.SaveChangesAsync();

            return existingTransaction;
        }

       
    }
}
