
namespace backend_dotnet7.Core.DTOs
{
    public class OrganizationExpenseDto
    {
        public long Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ExpenseImage { get; set; }
        public long? OrganizationId { get; set; }
    }

    public class CreateOrganizationExpenseDto
    {
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ExpenseImage { get; set; }
        public long? OrganizationId { get; set; }
    }
}
