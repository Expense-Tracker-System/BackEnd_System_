using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }
        public string? BudgetName { get; set; }
        public double BudgetAmount { get; set; }
        public string? BudgetDescription { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual List<BExpense> Bexpenses { get; set; }
    }
}
