using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class Message : BaseEntity<long>
    {
        public string? SenderUserName { get; set; }
        public string? ReceiverUserName { get; set; }
        public string? Text { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string? UserId { get; set; }
        public ApplicationUser? applicationUser { get; set; }
    }
}
