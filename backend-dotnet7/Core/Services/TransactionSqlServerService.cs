using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;

namespace backend_dotnet7.Core.Services
{
    public class TransactionSqlServerService : ITransactionReposatory
    {
        private readonly ApplicationDbContext _context;

        public TransactionSqlServerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Transaction> AllTransaction(int CategoryId)
        {
            return _context.Transactions.Where(t => t.CategoryId == CategoryId).ToList();
        }

        public Transaction GetTransaction(int CategoryId, int id)
        {
            return _context.Transactions.FirstOrDefault(t => t.CategoryId == CategoryId && t.TransactionId == id);
        }

        public Transaction AddTransaction(int CategoryId, Transaction transaction)
        {
            transaction.CategoryId = CategoryId;
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return _context.Transactions.Find(transaction.TransactionId);
        }

    }
}
