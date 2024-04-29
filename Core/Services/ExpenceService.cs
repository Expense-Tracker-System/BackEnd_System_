using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities.AddTrEntity;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Services
{
    public class ExpenceService : IExpenceService
    {
        private readonly ApplicationDbContext _context;

        public ExpenceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseEntity> AddExpense(ExpenseEntity expense)
        {
            _context.ExpenseEntitys.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public IEnumerable<ExpenseEntity> GetExpenses()
        {
            return _context.ExpenseEntitys.ToList();
        }

        public bool DeleteExpense(int id)
        {
            var expense = _context.ExpenseEntitys.Find(id);
            if (expense != null)
            {
                _context.ExpenseEntitys.Remove(expense);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<ExpenseEntity> GetExpenseById(int id)
        {
            return await _context.ExpenseEntitys.FindAsync(id);
        }
    }
}
