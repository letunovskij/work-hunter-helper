namespace WorkHunter.Models.Dto.Settings;

public sealed record BannerSettingDto
{
    public required string Description { get; init; }

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public bool? IsActive { get; init; }
}
