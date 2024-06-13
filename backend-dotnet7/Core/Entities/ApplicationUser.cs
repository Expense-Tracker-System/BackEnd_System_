using Microsoft.AspNetCore.Identity;

namespace backend_dotnet7.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Roles { get; set; }
        public string? UserImage { get; set; }

        //[NotMapped]
        //public IList<string> Roles { get; set; }
    }
}
