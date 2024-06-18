namespace backend_dotnet7.Core.Entities
{
    public class Budget : BaseEntity<long>
    {
        public string? BudgetName { get; set; }
        public double BudgetAmount { get; set; }
        public string? BudgetDescription { get; set; }
        public string? UserName { get; set; }
    }
}
