using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend_dotnet7.Core.Entities.AddTrEntity;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IExpenceService
    {
        Task<ExpenseEntity> AddExpense(ExpenseEntity expense);
        IEnumerable<ExpenseEntity> GetExpenses();
        Task<ExpenseEntity> GetExpenseById(int id);
        bool DeleteExpense(int id);
    }
}
