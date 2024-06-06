using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.Budget;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.Services
{
    public class BudgetService : IBudgetService
    {
        protected readonly ApplicationDbContext dbContext;
        public BudgetService(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public async Task<List<Budget>> AddBudget(BudgetDto budget)
        {
            var newbudget = new Budget
                {
                BudgetName = budget.BudgetName,
                BudgetAmount = budget.BudgetAmount,
                BudgetDescription = budget.BudgetDescription
                };
            dbContext.Budgets.Add(newbudget);
            await dbContext.SaveChangesAsync();
            return await dbContext.Budgets.ToListAsync();
        }

        public async Task<List<Budget>?> DeleteBudget(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Budget>> GetAllBudgets()
        {
            throw new NotImplementedException();
        }

        public async Task<Budget?> GetSingleBudget(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Budget>?> UpdateBudget(int id, BudgetDto request)
        {
            throw new NotImplementedException();
        }
    }
}
