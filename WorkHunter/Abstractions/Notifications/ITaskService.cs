using WorkHunter.Models.Dto.Notifications;
using WorkHunter.Models.Enums;
using WorkHunter.Models.Views.Notifications;

namespace WorkHunter.Abstractions.Notifications;

public interface ITaskService
{
    Task<IReadOnlyList<UserTaskView>> GetAll(TaskStatus status, TaskType? type = null, DateTime? created = null);

    UserTaskView Get(int id);

    Task<UserTaskView> GetAsync(int id);

    Task<IReadOnlyList<UserTaskView>> GetForCurrentUser(TaskStatus status, DateTime? created = null);

    Task CompletedByHand(int id, string reason);

    Task CompletedBatchByHand(IEnumerable<int> ids, string reason);

    Task<bool> CreateTask(TaskType type, Guid wresponseId, UserTaskDto? dto = null);

    Task SendReminderNotifications();
}
