using backend_dotnet7.Core.Dtos.Log;
using Dapper.Contrib.Extensions;
using System;

namespace backend_dotnet7.Core.Entities
{
    public class TransactionEntity
    {
        [Key]
        public int Id { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string userName { get; set; }

    }
}
