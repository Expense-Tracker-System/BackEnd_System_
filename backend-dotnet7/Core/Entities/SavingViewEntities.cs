namespace backend_dotnet7.Core.Entities
{
    public class SavingViewEntities
    {
        public int Id { get; set; }

        public int Amount { get; set; }
        public string Description { get; set; }


        public string BankName { get; set; }
        
        public DateTime? Date { get; set; }

        public string ? userName { get; set; }

    }
}
