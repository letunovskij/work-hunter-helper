namespace WorkHunter.Models.Dto.WorkHunters;

public sealed class WResponseCreateDto : WResponseUpdateDto
{
    /// <summary>
    /// Ссылка на вакансию (hh, linkedin, monster.com и т.д.)
    /// </summary>
    public required string VacancyUrl { get; set; }
}
