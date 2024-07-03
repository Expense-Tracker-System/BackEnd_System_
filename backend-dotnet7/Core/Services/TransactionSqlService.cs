using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class TransactionSqlService : ITransactionReposatory
    {
        private readonly ApplicationDbContext _context;

        public TransactionSqlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Transaction> AllTransaction(int CategoryId)
        {
            return _context.Transactions.Where(t => t.CategoryId == CategoryId).ToList();
        }

        public Transaction GetTransaction(int CategoryId, int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.Id == id && t.CategoryId == CategoryId);
        }
    }
}
