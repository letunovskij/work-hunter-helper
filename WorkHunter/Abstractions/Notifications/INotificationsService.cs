using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;
namespace WorkHunter.Abstractions.Notifications;

public interface INotificationsService
{
    Task<bool> SendAboutWresponseChanged(string to, WResponse wresponse);
    Task<bool> SendTaskReminder(UserTask userTask, User? responsible, string reminderNotification);
}
