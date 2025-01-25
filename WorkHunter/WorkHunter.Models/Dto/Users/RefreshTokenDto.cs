namespace WorkHunter.Models.Dto.Users;

public sealed record RefreshTokenDto
{
    public required string Token { get; init; }
}
