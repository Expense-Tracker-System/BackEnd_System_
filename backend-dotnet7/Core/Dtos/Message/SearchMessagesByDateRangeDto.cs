namespace backend_dotnet7.Core.Dtos.Message
{
    public class SearchMessagesByDateRangeDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public DateTime AdjustedToDate => ToDate.Date.AddDays(1).AddTicks(-1); // End of the day
    }
}
