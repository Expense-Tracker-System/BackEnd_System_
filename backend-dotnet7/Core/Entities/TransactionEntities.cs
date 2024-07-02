using backend_dotnet7.Core.Dtos.Log;
using System;

namespace backend_dotnet7.Core.Entities
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string userName { get; set; }

    }
}
