using backend_dotnet7.Core.Dtos.BExpense;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IBExpenseService
    {
        Task<List<BExpense>> GetAllBExpenses();
        Task<List<BExpense>> AddBExpense(BExpenseDto budget);
        Task<List<BExpense>?> UpdateBExpense(int id, BExpenseDto request);
        Task<List<BExpense>?> DeleteBExpense(int id);
    }
}
