using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class OrganizationIncome : BaseEntity<long>
    {
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }
        public string? Category { get; set; }

        [ForeignKey(nameof(Organization))]
        public long? OrganizationId { get; set; }
        // navigate
        public Organization? organization { get; set; }
    }
}
