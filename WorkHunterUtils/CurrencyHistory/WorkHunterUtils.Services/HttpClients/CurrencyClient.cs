using Common.Exceptions;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WorkHunterUtils.Abstractions.HttpClients;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;
using WorkHunterUtils.Models.Models;
using WorkHunterUtils.Models.Options;
using WorkHunterUtils.Services.Currencies;

namespace WorkHunterUtils.Services.HttpClients
{
    public sealed class CurrencyClient : ICurrencyClient
    {
        private readonly HttpClient httpClient;
        private readonly CurrencyOptions currencyOptions;
        private readonly ILogger<CurrencyClient> logger;

        public CurrencyClient(HttpClient httpClient, IOptionsMonitor<CurrencyOptions> taskOptions, ILogger<CurrencyClient> logger)
        {
            this.httpClient = httpClient;
            this.currencyOptions = taskOptions.CurrentValue;
            this.logger = logger;
        }

        public Uri GetBaseAdrress() 
            => httpClient.BaseAddress != null ? this.httpClient.BaseAddress : throw new BusinessErrorException($"{nameof(CurrencyService)} is not configured");

        public bool IsRubChangedSignificantly(CurrencyHistory? previousRateDocument, CurrencyRate currentCurrencyRate)
        {
            if (previousRateDocument != null)
            {
                var previousRate = JsonSerializer.Deserialize<CurrencyRate>(previousRateDocument.CurrenciesValues);

                if (previousRate != null)
                {
                    if (Math.Abs(previousRate.conversion_rates.RUB - currentCurrencyRate.conversion_rates.RUB) >= currencyOptions.DiffThreshold)
                        return true;
                }
            }

            return false;
        }

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
                using var response = await httpClient.GetAsync($"{currencyOptions.BaseUrl}/USD");
                response.EnsureSuccessStatusCode();
                responseText = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Get data error from {URL}", $"{currencyOptions.BaseUrl}/USD");
                throw;
            }

            if (!string.IsNullOrEmpty(responseText))
            {
                result = JsonSerializer.Deserialize<CurrencyRate>(responseText);
            } 

            if (result != null)
                return result;
            else
                throw new BusinessErrorException($"Get data error from {currencyOptions.BaseUrl}/USD");
        }
    }
}
