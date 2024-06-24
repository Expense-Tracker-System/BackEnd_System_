using System.ComponentModel.DataAnnotations.Schema;

namespace backend_dotnet7.Core.Entities
{
    public class Reminder
    {
        public int ReminderId { get; set; }
        public string? ReminderName { get; set; }
        public double ReminderAmount { get; set; }
        public string? ReminderDescription { get; set; }
        public DateTime ReminderstartDate { get; set; }
        public DateTime ReminderendDate { get; set; }

        [ForeignKey(nameof(User))]
        public string? UserName { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
