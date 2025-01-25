namespace WorkHunter.Models.Config;

public sealed class AuthOptions
{
    public required string Issuer { get; set; }

    public required string Audience { get; set; }

    public required string Key { get; set; }

    public int AccessTokenLifetime { get; set; }

    public int RefreshTokenLifetime { get; set; }
}
