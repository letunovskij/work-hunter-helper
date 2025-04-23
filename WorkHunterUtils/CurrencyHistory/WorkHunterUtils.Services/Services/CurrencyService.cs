using Microsoft.Extensions.Logging;
using System.Text.Json;
using WorkHunterUtils.Abstractions.Currencies;
using WorkHunterUtils.Abstractions.HttpClients;
using WorkHunterUtils.Data;
using WorkHunterUtils.Services.HttpClients;

namespace WorkHunterUtils.Services.Currencies;

public sealed class CurrencyService : ICurrencyService
{
    private readonly ICurrencyClient currencyClient;
    private readonly IWorkHunterUtilsDb workHunterUtilsDb;
    private readonly ILogger<CurrencyService> logger;

    public CurrencyService(ICurrencyClient currencyClient, IWorkHunterUtilsDb workHunterUtilsDb, ILogger<CurrencyService> logger)
    {
        this.currencyClient = currencyClient;
        this.workHunterUtilsDb = workHunterUtilsDb;
        this.logger = logger;
    }

    public async Task GetCurrencies()
    {
        try
        {
            var currencyRate = await currencyClient.GetCurrencies();

            workHunterUtilsDb.CurrencyHistories.Add(new()
            {
                CreateDate = DateTime.UtcNow,
                RequestDate = DateTime.UtcNow,
                CurrenciesValues = JsonDocument.Parse(JsonSerializer.Serialize(currencyRate))
            });

            await workHunterUtilsDb.SaveChangesAsync();
        } 
        catch (Exception ex)
        {
            logger.LogError(ex, "Get currencyRate error from {URL}", $"{currencyClient.GetBaseAdrress()}/USD");
        }
    }
}
