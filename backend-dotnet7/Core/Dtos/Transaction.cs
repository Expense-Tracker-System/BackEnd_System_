namespace backend_dotnet7.Core.Dtos
{
    public class Transaction
    {
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
