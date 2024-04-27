namespace backend_dotnet7.Core.Dtos.AddTran
{
    public class Expense
    {


        // Define properties of the Expense entity
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
