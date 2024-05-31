using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.DbContext;
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

        public async Task<TransactionEntities> AddTransaction(TransactionEntities transaction)
        {
            _context.TransactionEntities.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public IEnumerable<TransactionEntities> GetTransactions()
        {
            return _context.TransactionEntities.AsNoTracking().ToList();
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            var transaction = await _context.TransactionEntities.FindAsync(id);
            if (transaction == null)
            {
                return false;
            }

            _context.TransactionEntities.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TransactionEntities> UpdateTransaction(int id, TransactionEntities transaction)
        {
            var existingTransaction = await _context.TransactionEntities.FindAsync(id);
            if (existingTransaction == null)
            {
                return null;
            }

            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Description = transaction.Description;
            existingTransaction.Date = transaction.Date;

            _context.TransactionEntities.Update(existingTransaction);
            await _context.SaveChangesAsync();

            return existingTransaction;
        }
    }
}
