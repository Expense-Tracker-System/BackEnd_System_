using backend_dotnet7.Core.Interfaces;
using Quartz;

namespace backend_dotnet7.Core.BackgroundJobs
{
    public class DailyJob : IJob
    {
        private readonly IReminderService _reminderservice;
        public DailyJob(IReminderService reminderservice)
        {
            _reminderservice = reminderservice;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Overdue Job is running");
            await _reminderservice.ReminderMail();
            Console.WriteLine("Overdue Job is finished");
        }
    }
}
