namespace backend_dotnet7.Core.Entities
{
    public class TransactionEntities
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; } 
    }
}
