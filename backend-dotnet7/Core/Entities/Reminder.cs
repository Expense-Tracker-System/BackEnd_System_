namespace backend_dotnet7.Core.Entities
{
    public class Reminder
    {
        public int ReminderId { get; set; }
        public string ReminderName { get; set; } = string.Empty;
        public double RAmount { get; set; }
        public DateOnly ReminderDate { get; set; }
        public string RDescription { get; set; } = string.Empty;
    }
}
