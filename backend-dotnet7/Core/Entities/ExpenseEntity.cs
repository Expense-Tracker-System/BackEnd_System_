﻿namespace backend_dotnet7.Core.Entities
{
    public class ExpenseEntity
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.MinValue;
    }
}
