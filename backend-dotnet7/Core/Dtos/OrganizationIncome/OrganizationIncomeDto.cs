namespace backend_dotnet7.Core.Dtos.OrganizationIncome
{
    public class OrganizationIncomeDto
    {
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }
        public string? Category { get; set; }
        public long? OrganizationId { get; set; }
    }
}
