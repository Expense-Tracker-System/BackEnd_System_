using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class BExpense
    {
        public int BExpenseId { get; set; }
        public string? BExpenseName { get; set; }
        public double BExpenseAmount { get; set; }

        [ForeignKey(nameof(Budget))]
        public int BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
    }
}
