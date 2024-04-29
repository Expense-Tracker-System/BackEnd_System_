namespace backend_dotnet7.Core.Dtos.AddTran
{
    public class Income
    {
      
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
