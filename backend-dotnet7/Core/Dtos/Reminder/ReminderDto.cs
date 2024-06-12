namespace backend_dotnet7.Core.Dtos.Reminder
{
    public class ReminderDto
    {
        public string ReminderName { get; set; }
        public double ReminderAmount { get; set; }
        public string ReminderDescription { get; set; }
        public DateTime ReminderDate { get; set; } = DateTime.Now;
    }
}
