namespace backend_dotnet7.Core.Dtos
{
    public class savingViewDTO
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public string BankName { get; set; }

        public DateTime? Date { get; set; }

        public string ? userName { get; set; }


        public savingViewDTO(int id,int amount,string bankName,DateTime? date,string userName)
        {
            Id = id;
            Amount = amount;
            BankName = bankName;
            Date = date;
            this.userName = userName; 
        }
    }
}
