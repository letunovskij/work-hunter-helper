using MediatR;
using Microsoft.Extensions.Logging;

namespace WorkHunter.Services.MediatrNotificationsHandlers;

public abstract class BaseNotificationHandler<TNotification> : INotificationHandler<TNotification> where TNotification : INotification
{
    private readonly ILogger logger;

    protected BaseNotificationHandler(ILogger logger) {  this.logger = logger; }

    public abstract Task HandleCommand(TNotification notification, CancellationToken cancellationToken);

    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            await HandleCommand(notification, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during command {TypeName} execution", notification.GetType().Name);
        }
    }
}
