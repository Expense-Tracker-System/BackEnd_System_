using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public string? ReportCategory { get; set; }
        public string? ReportURL { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
