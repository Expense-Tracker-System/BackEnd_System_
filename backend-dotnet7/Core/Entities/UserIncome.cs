using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class UserIncome : BaseEntity<long>
    {
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }
        public string? Category { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
