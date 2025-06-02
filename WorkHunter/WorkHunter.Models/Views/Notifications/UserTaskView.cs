using WorkHunter.Models.Enums;
using WorkHunter.Models.Views.Users;
using WorkHunter.Models.Views.WorkHunters;

namespace WorkHunter.Models.Views.Notifications;

public sealed class UserTaskView
{
    public required UserBaseView User { get; set; }

    public int Id { get; set; }

    public int TypeId { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Completed { get; set; }

    public UserTaskStatus Status { get; set; }

    public bool IsCompletedByHand { get; set; }

    public string? CompletionReason { get; set; }

    public required string Text { get; set; }

    public DateTime LastNotificationDate { get; set; }

    public Guid WResponseId { get; set; }

    public WResponseView? WResponse { get; set; }
}
