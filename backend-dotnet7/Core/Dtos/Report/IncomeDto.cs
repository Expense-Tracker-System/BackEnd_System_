namespace backend_dotnet7.Core.Dtos.Report
{
    public class IncomeDTO
    {
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime CreatedDate2 { get; set;}
    }

}
