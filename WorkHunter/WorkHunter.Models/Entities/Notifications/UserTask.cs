using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Entities.WorkHunters;
using WorkHunter.Models.Enums;

namespace WorkHunter.Models.Entities.Notifications;

public sealed class UserTask
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public UserTaskType? Type { get; set; }

    public string? ResponsibleId { get; set; }

    public User? Responsible { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Completed { get; set; }

    public UserTaskStatus Status { get; set; }

    public bool IsCompletedByHand {  get; set; }

    public string? CompletionReason { get; set; }

    public required string Text { get; set; }

    public DateTime LastNotificationDate { get; set; }

    public Guid WResponseId { get; set; }

    public WResponse? WResponse { get; set; }
}
