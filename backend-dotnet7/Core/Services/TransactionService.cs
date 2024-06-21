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

        public async Task<TransactionDto> AddTransaction(TransactionDto transaction, string userName)
        {
            var entity = new TransactionEntity
            {
                Amount = transaction.Amount,
                Description = transaction.Description,
                userName = userName, // Assign the UserName from DTO
                CreatedAt = DateTime.Now // Or use transaction.getLogDto.CreatedAt
            };

            _context.TransactionEntities.Add(entity);
            await _context.SaveChangesAsync();

            transaction.Id = entity.Id; // Set the ID of the created transaction
            return transaction;
        }

        public IEnumerable<TransactionDto> GetTransactions(string userName)
        {
            var transactions = _context.TransactionEntities.Where(t => t.userName == userName).Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Description = t.Description,
                    Date = t.CreatedAt, // Assuming CreatedAt as Date
                    userName = userName
                })
                .ToList();

            return transactions;
        }

        public async Task<bool> DeleteTransaction(int id, string userName)
        {
            var transaction = await _context.TransactionEntities
                .FirstOrDefaultAsync(t => t.Id == id && t.userName == userName);

            if (transaction == null)
                return false;

            _context.TransactionEntities.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionDto> UpdateTransaction(int id, TransactionDto transactionDto, string userName)
        {
            var existingTransaction = await _context.TransactionEntities
                .FirstOrDefaultAsync(t => t.Id == id && t.userName == userName);

            if (existingTransaction == null)
                return null;

            existingTransaction.Amount = transactionDto.Amount;
            existingTransaction.Description = transactionDto.Description;
            existingTransaction.CreatedAt = transactionDto.Date;

            _context.TransactionEntities.Update(existingTransaction);
            await _context.SaveChangesAsync();

            return transactionDto;
        }

       
    }
}
