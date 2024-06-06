namespace backend_dotnet7.Core.Entities
{
    public class Reminder : BaseEntity<long>
    {
        public int ReminderId {  get; set; }
        public string ReminderName { get; set; }
        public string ReminderDescription { get; set; }
        public DateTime ReminderDate { get; set; } = DateTime.Now;
    }
}
