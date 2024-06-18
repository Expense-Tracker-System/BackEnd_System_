namespace backend_dotnet7.Core.Entities
{
    public class Budget 
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public double BudgetAmount { get; set; }
        public string BudgetDescription { get; set; }
    }
}
