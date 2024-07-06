using backend_dotnet7.Core.Dtos.Report;

namespace backend_dotnet7.Core.Interfaces
{
    public interface IFinancialService
    {
        Task<FinancialDataDTO> GetFinancialDataAsync(DateTime startDate, DateTime endDate);
    }
}
