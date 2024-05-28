using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Infrastructure.Repositories
{
    public class ExpenceService : IExpenseService
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

        public async Task<IEnumerable<ExpenseEntity>> GetExpenses()
        {
            return await _context.ExpenseEntitys.ToListAsync();
        }

        public async Task<ExpenseEntity> GetExpenseById(int id)
        {
            return await _context.ExpenseEntitys.FindAsync(id);
        }

        public async Task<ExpenseEntity> UpdateExpense(int id, ExpenseEntity expense)
        {
            var existingExpense = await _context.ExpenseEntitys.FindAsync(id);
            if (existingExpense == null)
            {
                return null;
            }

            existingExpense.Amount = expense.Amount;
            existingExpense.Description = expense.Description;
            existingExpense.UpdatedDate = DateTime.UtcNow;

            _context.ExpenseEntitys.Update(existingExpense);
            await _context.SaveChangesAsync();

            return existingExpense;
        }

        public async Task<bool> DeleteExpense(int id)
        {
            var expense = await _context.ExpenseEntitys.FindAsync(id);
            if (expense == null)
            {
                return false;
            }

            _context.ExpenseEntitys.Remove(expense);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
