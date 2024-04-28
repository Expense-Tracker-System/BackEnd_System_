namespace backend_dotnet7.Core.Entities.AddTrEntity
{
    public class IncomeEntity
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.MinValue;
    }
}
