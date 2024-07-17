using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class DeactivateUserAccount : BaseEntity<long>
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }

        public string? Message { get; set; }
        public DateTime Date { get; set; }
    }
}
