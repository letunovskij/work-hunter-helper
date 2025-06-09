using Common.Exceptions;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Views.Users;
using WorkHunter.Models.Views.WorkHunters;
using WorkHunterHelper.Models;

namespace WorkHunterHelper.Services;

public sealed class WorkHunterService : IWorkHunterService
{
    private readonly HttpClient httpClient;
    private readonly WorkHunterOptions workHunterOptions;
    private readonly ILogger<WorkHunterService> logger;

    private static readonly JsonSerializerOptions serializationOptions = new(JsonSerializerDefaults.Web);

    public WorkHunterService(HttpClient httpClient, 
        IOptionsMonitor<WorkHunterOptions> workHunterOptions, 
        ILogger<WorkHunterService> logger)
    {
        this.httpClient = httpClient;
        this.workHunterOptions = workHunterOptions.CurrentValue;
        this.logger = logger;
    }

    public Uri GetBaseAdrress()
        => httpClient.BaseAddress != null ? this.httpClient.BaseAddress 
                                          : throw new Exception($"{nameof(WorkHunterService)} is not configured");

    public async Task<string> GetToken(LoginDto dto)
    {
        var content = JsonSerializer.Serialize(dto, serializationOptions);

        var request = new HttpRequestMessage(HttpMethod.Post, $"{workHunterOptions.BaseUrl}users/token");
        var httpContent = new StringContent(content, Encoding.UTF8);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        if (string.IsNullOrEmpty(workHunterOptions.BaseUrl))
        {
            logger.LogError("CurrencyService is not configured");
            throw new BusinessErrorException($"{nameof(WorkHunterService)} is not configured");
        }

        string responseText = string.Empty;
        try
        {
            using var response = await httpClient.PostAsync($"{workHunterOptions.BaseUrl}users/token", httpContent);
            response.EnsureSuccessStatusCode();
            responseText = await response.Content.ReadAsStringAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get data error from {URL}", $"{workHunterOptions.BaseUrl}users/token");
            throw;
        }

        if (!string.IsNullOrEmpty(responseText))
        {
            var result = JsonSerializer.Deserialize<TokensView>(responseText);

            if (result != null)
                return result.AccessToken;
        }

        throw new BusinessErrorException($"Get data error from {workHunterOptions.BaseUrl}users/token");
    }

    public async Task<List<WResponseView>> GetResponses(string accessToken)
    {
        //var content = JsonSerializer.Serialize(dto, serializationOptions);

        var request = new HttpRequestMessage(HttpMethod.Get, $"{workHunterOptions.BaseUrl}responses");
        request.Content.Headers.Add("access-token", accessToken);
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        if (string.IsNullOrEmpty(workHunterOptions.BaseUrl))
        {
            logger.LogError("CurrencyService is not configured");
            throw new BusinessErrorException($"{nameof(WorkHunterService)} is not configured");
        }

        string responseText = string.Empty;
        try
        {
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            responseText = await response.Content.ReadAsStringAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get data error from {URL}", $"{workHunterOptions.BaseUrl}users/token");
            throw;
        }

        if (!string.IsNullOrEmpty(responseText))
        {
            var result = JsonSerializer.Deserialize<List<WResponseView>>(responseText);

            if (result != null)
                return result;
        }

        throw new BusinessErrorException($"Get data error from {workHunterOptions.BaseUrl}users/token");
    }
}
