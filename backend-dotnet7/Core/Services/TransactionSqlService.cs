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

        public List<Transaction> AllTransaction()
        {
            return _context.Transactions.ToList();
        }
    }
}
