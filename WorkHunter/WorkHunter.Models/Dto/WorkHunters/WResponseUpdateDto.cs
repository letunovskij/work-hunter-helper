namespace WorkHunter.Models.Dto.WorkHunters;

public class WResponseUpdateDto
{
    /// <summary>
    /// Есть ли ответ на отклик
    /// </summary>
    public bool? IsAnswered { get; set; }

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
}
