using System;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;
using WorkHunterUtils.Models.Models;

namespace WorkHunterUtils.Abstractions.HttpClients
{
    public interface ICurrencyClient
    {
        Task<CurrencyRate> GetCurrencies();

        bool IsRubChangedSignificantly(CurrencyHistory? previousRateDocument, CurrencyRate currentCurrencyRate);

        Uri GetBaseAdrress();
    }
}
