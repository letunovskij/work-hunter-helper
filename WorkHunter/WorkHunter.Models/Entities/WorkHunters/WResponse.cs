using WorkHunter.Models.Entities.Interviews;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Enums;

namespace WorkHunter.Models.Entities.WorkHunters;

/// <summary>
/// Отклик пользователя приложения WorkHunter на вакансию
/// </summary>
public sealed class WResponse : Entity
{
    public ResponseStatus Status { get; set; }

    /// <summary>
    /// WHunter user Id
    /// </summary>
    public required string UserId { get; set; }

    public User? User { get; set; }

    /// <summary>
    /// HeadHunter response Identificator (id отклика/приглашения HH)
    /// </summary>
    public string? HhId { get; set; }

    /// <summary>
    /// created_at
    /// </summary>
    public DateTime CreatedAt { get; set; }

    public bool IsAnswered { get; set; }

    public bool ViewedByMe { get; set; }

    /// <summary>
    /// Контакты работодателя
    /// </summary>
    public string? Contact { get; set; }

    /// <summary>
    /// Ответ работодателя на отклик
    /// </summary>
    public string? AnswerText { get; set; }

    /// <summary>
    /// Почта работодателя
    /// </summary>
    public string? Email { get; set; }

    public required string VacancyUrl { get; set; }

    public ICollection<VideoInterviewFile>? VideoInterviewFiles { get; set; }

    public ICollection<UserTask>? UserTasks { get; set; }
}