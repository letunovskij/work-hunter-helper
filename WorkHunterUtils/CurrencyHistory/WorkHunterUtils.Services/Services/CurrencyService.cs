using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WorkHunterUtils.Abstractions.Currencies;
using WorkHunterUtils.Abstractions.HttpClients;
using WorkHunterUtils.Abstractions.Services;
using WorkHunterUtils.Data;

namespace WorkHunterUtils.Services.Currencies;

public sealed class CurrencyService : ICurrencyService
{
    private readonly ICurrencyClient currencyClient;
    private readonly IWorkHunterUtilsDb workHunterUtilsDb;
    private readonly ILogger<CurrencyService> logger;
    private readonly IEmailService emailService;

    public CurrencyService(
        ICurrencyClient currencyClient, 
        IWorkHunterUtilsDb workHunterUtilsDb,
        IEmailService emailService,
        ILogger<CurrencyService> logger)
    {
        this.currencyClient = currencyClient;
        this.workHunterUtilsDb = workHunterUtilsDb;
        this.logger = logger;
        this.emailService = emailService;
    }

    public async Task GetCurrencies()
    {
        var previousRateDocument = await workHunterUtilsDb.CurrencyHistories.OrderBy(x => x.CreateDate).LastOrDefaultAsync();

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

            if (currencyClient.IsRubChangedSignificantly(previousRateDocument, currencyRate))
                await emailService.SendAboutVacancyCostChanged("olluntest@mail.ru", null, currencyRate);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get currencyRate error from {URL}", $"{currencyClient.GetBaseAdrress()}/USD");
        }
    }
}
