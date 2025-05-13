using System;
using WorkHunter.Models.Entities.WorkHunters;
namespace WorkHunter.Abstractions.Notifications;

public interface INotificationsService
{
    Task<bool> SendAboutWresponseChanged(string to, WResponse wresponse);
}
