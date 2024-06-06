namespace backend_dotnet7.Core.Entities
{
    public class Budget : BaseEntity<long>
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string BDescription { get; set; } = string.Empty;
    }
}
