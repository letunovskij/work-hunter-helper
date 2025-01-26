using WorkHunter.Models.Enums;

namespace WorkHunter.Models.Entities.WHunter;

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

    public string? Contact { get; set; }

    public string? AnswerText { get; set; }

    public string? Email { get; set; }

    public required string VacancyUrl { get; set; }
}