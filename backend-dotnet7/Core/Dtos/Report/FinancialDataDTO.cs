namespace backend_dotnet7.Core.Dtos.Report
{
    public class FinancialDataDTO
    {
        public double TotalIncomes { get; set; }
        public double TotalExpenses { get; set; }
        public double TotalBalance { get; set; }
        public List<ExpenseDTO> Expenses { get; set; }
        public List<IncomeDTO> Incomes { get; set; }
    }
}
