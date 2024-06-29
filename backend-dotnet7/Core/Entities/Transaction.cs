namespace backend_dotnet7.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Note { get; set; }
        public DateTime Created {  get; set; }
        public TransactionStatus Status { get; set; } //  New,Inprogress,Completed

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
