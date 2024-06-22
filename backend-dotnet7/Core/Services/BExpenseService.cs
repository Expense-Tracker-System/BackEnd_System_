using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.BExpense;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class BExpenseService : IBExpenseService
    {
        protected readonly ApplicationDbContext dbContext;
        public BExpenseService(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public async Task<List<BExpense>> AddBExpense(BExpenseDto bexpense)
        {
            var newbexpense = new BExpense
            {
                BExpenseName = bexpense.BExpenseName,
                BExpenseAmount = bexpense.BExpenseAmount,
                BudgetId = bexpense.BudgetId
            };
            dbContext.BExpenses.Add(newbexpense);
            await dbContext.SaveChangesAsync();
            return await dbContext.BExpenses.ToListAsync();
        }

        public async Task<List<BExpense>?> DeleteBExpense(int id)
        {
            var bexpense = await dbContext.BExpenses.FindAsync(id);

            if (bexpense == null)
                return null;

            dbContext.BExpenses.Remove(bexpense);
            await dbContext.SaveChangesAsync();
            return await dbContext.BExpenses.ToListAsync();
            
        }

        public async Task<List<BExpense>> GetAllBExpenses()
        {
            var bexpenses = await dbContext.BExpenses.ToListAsync();
            return bexpenses;
        }

        public async Task<List<BExpense>?> UpdateBExpense(int id, BExpenseDto request)
        {
            var bexpense = await dbContext.BExpenses.FindAsync(id);

            if (bexpense is null) return null;

            bexpense.BExpenseName = request.BExpenseName;
            bexpense.BExpenseAmount = request.BExpenseAmount;
            await dbContext.SaveChangesAsync();

            return await dbContext.BExpenses.ToListAsync();
        }

    }
}
