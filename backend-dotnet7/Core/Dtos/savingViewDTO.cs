namespace backend_dotnet7.Core.Dtos
{
    public class savingViewDTO
    {
        private string description1;

        public int Id { get; set; }

        public int Amount { get; set; }

        public string BankName { get; set; }

        public DateTime? Date { get; set; }

        public string ? userName { get; set; }

        public string Description { get; set; }
        public savingViewDTO(int id,int amount,string bankName,DateTime? date,string Description, string? userName)
        {
            Id = id;
            Amount = amount;
            BankName = bankName;
            Date = date;
            this.Description = Description;
            this.userName = userName; 
        }

       
    }
}
