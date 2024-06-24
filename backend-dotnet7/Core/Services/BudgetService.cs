using backend_dotnet7.Core.DbContext;
using backend_dotnet7.Core.Dtos.BExpense;
using backend_dotnet7.Core.Dtos.Budget;
using backend_dotnet7.Core.Entities;
using backend_dotnet7.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend_dotnet7.Core.Services
{
    public class BudgetService : IBudgetService
    {
        protected readonly ApplicationDbContext dbContext;
        public BudgetService(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public async Task<List<Budget>> AddBudget(BudgetDto budget, ClaimsPrincipal user)
        {
            var newbudget = new Budget
                {
                BudgetName = budget.BudgetName,
                BudgetAmount = budget.BudgetAmount,
                BudgetDescription = budget.BudgetDescription,
                UserName = budget.UserName // ahanna
                };
            dbContext.Budgets.Add(newbudget);
            await dbContext.SaveChangesAsync();
            return await dbContext.Budgets.ToListAsync();
        }

        public async Task<List<Budget>?> DeleteBudget(int id)
        {
            var budget = await dbContext.Budgets.FindAsync(id);

            if (budget is null)
                return null;

            dbContext.Budgets.Remove(budget);
            await dbContext.SaveChangesAsync();
            return await dbContext.Budgets.ToListAsync();
        }

        public async Task<List<getbudgetDto>> GetAllBudgets(string username)
        {
            var budgets = await dbContext.Budgets.Where(e=>e.UserName==username).ToListAsync();
            var getbudget = new List<getbudgetDto>();
            foreach (var budget in budgets)
            {
                var expenses=await dbContext.BExpenses.Where(e=>e.BudgetId==budget.BudgetId).ToListAsync();
                var expensesdto=new List<BExpenseDto>();
                foreach(var expense in expenses)
                {
                    var b = new BExpenseDto
                    {
                        BudgetId = expense.BExpenseId,
                        BExpenseAmount = expense.BExpenseAmount,
                        BExpenseName = expense.BExpenseName,
                        ExpenseId = expense.BExpenseId,
                    };
                    expensesdto.Add(b);
                }
                var totalExpense = await dbContext.BExpenses
                                    .Where(e => e.BudgetId == budget.BudgetId)
                                    .SumAsync(e => e.BExpenseAmount);

                var a = new getbudgetDto
                {
                    Id = budget.BudgetId,
                    budgetName = budget.BudgetName,
                    budgetAmount = budget.BudgetAmount,
                    budgetDescription = budget.BudgetDescription,
                    remain = budget.BudgetAmount -totalExpense,
                    spent = totalExpense,
                    expenses = expensesdto,

                };
                getbudget.Add(a);
            }
            return getbudget;
        }

        public async Task<getbudgetDto?> GetSingleBudget(int id)
        {
            var budget = await dbContext.Budgets.FindAsync(id);

            if (budget is null) return null;

            var expenses = await dbContext.BExpenses.Where(e => e.BudgetId == budget.BudgetId).ToListAsync();
            var expensesdto = new List<BExpenseDto>();
            foreach (var expense in expenses)
            {
                var b = new BExpenseDto
                {
                    BudgetId = expense.BudgetId,
                    BExpenseAmount = expense.BExpenseAmount,
                    BExpenseName = expense.BExpenseName,
                    ExpenseId=expense.BExpenseId
                };
                expensesdto.Add(b);
            }
            var totalExpense = await dbContext.BExpenses
                                .Where(e => e.BudgetId == budget.BudgetId)
                                .SumAsync(e => e.BExpenseAmount);

            var a = new getbudgetDto
            {
                Id = budget.BudgetId,
                budgetName = budget.BudgetName,
                budgetAmount = budget.BudgetAmount,
                budgetDescription = budget.BudgetDescription,
                remain = budget.BudgetAmount - totalExpense,
                spent = totalExpense,
                expenses = expensesdto,

            };

            return a;
        }

        public async Task<List<Budget>?> UpdateBudget(int id, BudgetDto request)
        {
            var budget = await dbContext.Budgets.FindAsync(id);

            if (budget is null) return null;

            budget.BudgetName = request.BudgetName;
            budget.BudgetAmount = request.BudgetAmount;
            budget.BudgetDescription = request.BudgetDescription;
            await dbContext.SaveChangesAsync();

            return await dbContext.Budgets.ToListAsync();
        }
    }
}
