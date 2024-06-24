namespace backend_dotnet7.Core.Dtos.BExpense
{
    public class BExpenseDto
    {
        public string? BExpenseName { get; set; }
        public double BExpenseAmount { get; set; }
        public int BudgetId { get; set; }

        public int ExpenseId{get; set; }

    }
}
