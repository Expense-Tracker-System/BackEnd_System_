namespace backend_dotnet7.Core.Entities
{
    public class Reminder : BaseEntity<long>
    {
        public string? ReminderName { get; set; }
        public double ReminderAmount { get; set; }
        public string? ReminderDescription { get; set; }
        public DateTime ReminderstartDate { get; set; }
        public DateTime ReminderendDate { get; set; }
        public string? UserName { get; set; }
    }
}
