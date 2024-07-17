using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class Organization : BaseEntity<long>
    {
        //[Key]
        //public long Id { get; set; }
        public string? Name { get; set; }
        public int MembersCount { get; set; }
        public double TotalTakeAmount { get; set; }
        public double TotalGetAmount { get; set; }
        public string? LeaderUsername { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }

        public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
        public ICollection<OrganizationIncome> OrganizationIncomes { get; set; } = new List<OrganizationIncome>();
        public ICollection<OrganizationExpense> OrganizationExpenses { get; set; } = new List<OrganizationExpense>();
    }
}
