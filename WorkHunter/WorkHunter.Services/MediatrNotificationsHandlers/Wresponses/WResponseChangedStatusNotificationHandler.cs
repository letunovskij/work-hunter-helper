using Abstractions.Users;
using Microsoft.Extensions.Logging;
using WorkHunter.Abstractions.Notifications;
using WorkHunter.Models.MediatrNotifications.Wresponses;

namespace WorkHunter.Services.MediatrNotificationsHandlers.Wresponses;

public sealed class WResponseChangedStatusNotificationHandler : BaseNotificationHandler<WResponseChangedStatusNotification>
{
    private readonly IUserService userService;

    private readonly INotificationsService notificationsService;

    public WResponseChangedStatusNotificationHandler(
        IUserService userService,
        INotificationsService notificationsService,
        ILogger<WResponseChangedStatusNotificationHandler> logger) : base(logger)
    {
        this.userService = userService;
        this.notificationsService = notificationsService;
    }

    public async override Task HandleCommand(WResponseChangedStatusNotification notification, CancellationToken cancellationToken)
    {
        var currentUser = await userService.GetCurrent();

        await notificationsService.SendAboutWresponseChanged(currentUser.Email, notification.Wresponse);
    }
}
