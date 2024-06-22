using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class UserOrganization
    {
        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        // navigate
        public ApplicationUser? applicationUser { get; set; }

        [ForeignKey(nameof(Organization))]
        public long? OrganizationId { get; set; }
        // navigate
        public Organization? organization { get; set; }

        public string? Position { get; set; }
        public int shares { get; set; }
        public double TakeAmount { get; set; }
        public double GetAmount { get; set; }
    }
}
