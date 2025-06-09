namespace WorkHunter.Models.Entities.Users;

public sealed class UserSession
{
    public Guid Id { get; set; }

    public required string UserId { get; set; }

    public User? User { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime LoginDate { get; set; }
}
