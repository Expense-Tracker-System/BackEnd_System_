using System.ComponentModel.DataAnnotations;

namespace backend_dotnet7.Core.Entities
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }
        public string? BudgetName { get; set; }
        public double BudgetAmount { get; set; }
        public string? BudgetDescription { get; set; }
        public string? UserName { get; set; }

        public virtual List<BExpense> Bexpenses{ get; set;}
    }
}
