using EmailSender.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkHunter.Abstractions.Notifications;
using WorkHunter.Models.Config;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Services.Notifications;

public sealed class NotificationsService : BaseEmailSender<EmailOptions>, INotificationsService
{
    public NotificationsService(IOptionsSnapshot<EmailOptions> emailOptions, ILogger<NotificationsService> logger) : base(emailOptions.Value, logger)
    {
    }

    public async Task<bool> SendAboutWresponseChanged(string to, WResponse wresponse)
    {
        var emailBody = $"Статус отклика {wresponse.Id} изменился на {wresponse.Status}";
        var isSent = await base.Send("olluntest@mail.ru", $"Статус отклика {wresponse.Id} изменился", emailBody);

        return isSent;
    }

    public async Task<bool> SendTaskReminder(UserTask userTask, User? responsible, string reminderNotification)
    {
        if (responsible == null || string.IsNullOrEmpty(responsible.Email))
        {
            this.logger.LogError("Не указан пользователь по задаче {UserTaskId}!", userTask.Id);
            return false;
        }

        var emailBody = reminderNotification;
        var isSent = await base.Send(responsible.Email, $"Напоминание о действиях {userTask.Id} по отклику: ", emailBody);

        return isSent;
    }
}
