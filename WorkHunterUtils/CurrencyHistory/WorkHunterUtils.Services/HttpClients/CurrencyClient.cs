using Common.Exceptions;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WorkHunterUtils.Abstractions.HttpClients;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;
using WorkHunterUtils.Models.Options;
using WorkHunterUtils.Services.Currencies;

namespace WorkHunterUtils.Services.HttpClients
{
    public sealed class CurrencyClient : ICurrencyClient
    {
        private readonly HttpClient httpClient;
        private readonly CurrencyOptions taskOptions;
        private readonly ILogger<CurrencyClient> logger;

        public CurrencyClient(HttpClient httpClient, IOptionsMonitor<CurrencyOptions> taskOptions, ILogger<CurrencyClient> logger)
        {
            this.httpClient = httpClient;
            this.taskOptions = taskOptions.CurrentValue;
            this.logger = logger;
        }

        public Uri GetBaseAdrress() 
            => httpClient.BaseAddress != null ? this.httpClient.BaseAddress : throw new BusinessErrorException($"{nameof(CurrencyService)} is not configured");

        public async Task<CurrencyRate> GetCurrencies()
        {
            CurrencyRate? result = null;

            if (httpClient.IsInvalidHost())
            {
                logger.LogError("CurrencyService is not configured");
                throw new BusinessErrorException($"{nameof(CurrencyService)} is not configured");
            }

            string responseText = string.Empty;
            try
            {
                using var response = await httpClient.GetAsync($"{taskOptions.BaseUrl}/USD");
                response.EnsureSuccessStatusCode();
                responseText = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get data error from {URL}", $"{taskOptions.BaseUrl}/USD");
                throw;
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                result = JsonSerializer.Deserialize<CurrencyRate>(responseText);
            } 

            if (result != null)
                return result;
            else
                throw new BusinessErrorException($"Get data error from {taskOptions.BaseUrl}/USD");
        }
    }
}
