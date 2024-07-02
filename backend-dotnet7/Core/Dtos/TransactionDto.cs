using System;
using backend_dotnet7.Core.Dtos.Log;

namespace backend_dotnet7.Core.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? userName { get; set; }
    }
}
