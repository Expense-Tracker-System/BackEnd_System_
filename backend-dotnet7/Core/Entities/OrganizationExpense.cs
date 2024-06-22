using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class OrganizationExpense : BaseEntity<long>
    {
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ExpenseImage { get; set; }

        [ForeignKey(nameof(Organization))]
        public long? OrganizationId { get; set; }
        // navigate
        public Organization? organization { get; set; }
    }
}
