using System.Text.Json.Serialization;

namespace WorkHunter.Models.Views.Users;

public sealed class TokensView
{
    [JsonPropertyName("accessToken")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; }
}
