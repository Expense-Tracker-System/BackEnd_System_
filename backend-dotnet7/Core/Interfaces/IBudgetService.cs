using backend_dotnet7.Core.Dtos.Budget;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetAllBudgets();
        Task<Budget?> GetSingleBudget(int id);
        Task<List<Budget>> AddBudget(BudgetDto budget);
        Task<List<Budget>?> UpdateBudget(int id, BudgetDto request);
        Task<List<Budget>?> DeleteBudget(int id);
    }
}
