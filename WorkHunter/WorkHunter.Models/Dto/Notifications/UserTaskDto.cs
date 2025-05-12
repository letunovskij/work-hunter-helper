using WorkHunter.Models.Dto.WorkHunters;

namespace WorkHunter.Models.Dto.Notifications;

public sealed class UserTaskDto
{
    public string? UserId { get; set; }

    public Guid WResponseId { get; set; }

    public WResponseCreateDto? WResponse { get; set; }
}
