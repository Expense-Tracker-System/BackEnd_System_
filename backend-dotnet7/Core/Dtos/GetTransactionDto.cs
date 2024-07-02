namespace backend_dotnet7.Core.Dtos
{
    public class GetTransactionDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
    }
}
