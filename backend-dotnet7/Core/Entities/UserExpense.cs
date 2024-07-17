using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class UserExpense : BaseEntity<long>
    {
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ExpenseImage { get; set; }
        
        public DateTime CreatedDate2 { get; set; } = DateTime.Now;

        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
