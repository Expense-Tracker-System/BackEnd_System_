using backend_dotnet7.Core.Entities.AddTrEntity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IExpenseService
    {
        Task<IncomeEntity> AddExpense(IncomeEntity expense);
        IEnumerable<IncomeEntity> GetExpenses();
        bool DeleteExpense(int id);
        Task<IncomeEntity> GetExpenseById(int id);
    }
}
