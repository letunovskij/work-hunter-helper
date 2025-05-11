using WorkHunter.Models.Enums;

namespace WorkHunter.Models.Entities.Notifications;

public sealed class UserTaskType
{
    public int Id { get; set; }

    public TaskType Type { get; set; }

    /// <summary>
    /// Наименование задачи
    /// </summary>
    public required string TaskName { get; set; }

    public required string TaskText { get; set; }

    /// <summary>
    /// Тема письма на почту в момент создания задачи
    /// </summary>
    public string? InitialNotificationSubject { get; set; }

    /// <summary>
    /// Текст нотификации на почту
    /// </summary>
    public string? InitialNotificationText { get; set; }

    /// <summary>
    /// Получатель письма
    /// </summary>
    public string Recipient {  get; set; }

    /// <summary>
    /// Частота напоминаний
    /// </summary>
    public int RemindersFrequency { get; set; }

    /// <summary>
    /// Активна ли задачи
    /// </summary>
    public bool IsActive { get; set; }

    public ICollection<UserTask>? UserTasks { get; set; }
}
