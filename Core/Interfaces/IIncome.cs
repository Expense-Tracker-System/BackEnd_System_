using backend_dotnet7.Core.Entities.AddTrEntity;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IIncome
    {


        Task<IncomeEntity> AddIncome(IncomeEntity expense);
        IEnumerable<IncomeEntity> GetIncome();
        bool DeleteIncome(int id);
        Task<IncomeEntity> GetIncomeById(int id);
    }
}
