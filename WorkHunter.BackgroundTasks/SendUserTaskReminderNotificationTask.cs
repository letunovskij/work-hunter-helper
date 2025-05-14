using Common.BackgroundTasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkHunter.Abstractions.Notifications;
using WorkHunter.Models.Config;

namespace WorkHunter.BackgroundTasks
{
    public class SendUserTaskReminderNotificationTask : BaseCronTask<ITaskService, SendUserTaskReminderNotificationOptions>
    {
        public override Func<ITaskService, Task> Action => (service) => service.SendReminderNotifications();

        public SendUserTaskReminderNotificationTask(IOptionsMonitor<SendUserTaskReminderNotificationOptions> options,
            IServiceProvider services,
            ILogger<SendUserTaskReminderNotificationTask> logger) : base(options, services, logger) { }
    }
}
