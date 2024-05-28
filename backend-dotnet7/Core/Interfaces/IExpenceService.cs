using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Entities;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseEntity> AddExpense(ExpenseEntity expense);
        Task<IEnumerable<ExpenseEntity>> GetExpenses();
        Task<ExpenseEntity> GetExpenseById(int id);
        Task<ExpenseEntity> UpdateExpense(int id, ExpenseEntity expense);
        Task<bool> DeleteExpense(int id);
    }
}
