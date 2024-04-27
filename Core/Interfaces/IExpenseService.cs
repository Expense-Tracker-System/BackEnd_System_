using backend_dotnet7.Core.Entities.AddTrEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseEntity> AddExpense(ExpenseEntity expense);
        IEnumerable<ExpenseEntity> GetExpenses();
        bool DeleteExpense(int id);
        Task<ExpenseEntity> GetExpenseById(int id);
    }
}
