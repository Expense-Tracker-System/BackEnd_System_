namespace backend_dotnet7.Core.Entities
{
    public class Organization : BaseEntity<long>
    {
        public string? Name { get; set; }
        public int MembersCount { get; set; }
        public double TotalTakeAmount { get; set; }
        public double TotalGetAmount { get; set; }
        public string? LeaderUserName { get; set; }

        // navigate
        public ICollection<UserOrganization> userOrganizations { get; set; } = new List<UserOrganization>();
        public ICollection<OrganizationIncome> organizationIncomes { get; set; } = new List<OrganizationIncome>();
        public ICollection<OrganizationExpense> organizationExpenses { get; set; } = new List<OrganizationExpense>();
        
    }
}
