using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class Log : BaseEntity<long>
    {
        public string? UserName { get; set; }
        public string? Description { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
