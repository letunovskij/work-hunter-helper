using WorkHunter.Models.Enums;
using WorkHunter.Models.Views.Users;

namespace WorkHunter.Models.Views.WorkHunters;

public sealed class WResponseView
{
    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }

    public ResponseStatus Status { get; set; }

    public string? HhId { get; set; }

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

    public required UserBaseView User { get; set; }
}
