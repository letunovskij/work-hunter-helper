using System.Text.Json.Serialization;

namespace WorkHunter.Models.Dto.Users;

public sealed class LoginDto
{
    [JsonPropertyName("email")]
    public required string Email { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }
}
