using MediatR;
using WorkHunter.Models.Entities.WorkHunters;

namespace WorkHunter.Models.MediatrNotifications.Wresponses;

public sealed class WResponseChangedStatusNotification : INotification
{
    public required WResponse Wresponse { get; set; }
}
