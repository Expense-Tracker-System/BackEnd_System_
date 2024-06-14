namespace backend_dotnet7.Core.Entities
{
    public class Reminder 
    {
        public int ReminderId {  get; set; }
        public string ReminderName { get; set; }
        public double ReminderAmount { get; set; }
        public string ReminderDescription { get; set; }
        public DateTime ReminderDate { get; set; } = DateTime.Now;
    }
}
