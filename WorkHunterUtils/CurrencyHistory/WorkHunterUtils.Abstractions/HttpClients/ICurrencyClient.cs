using System;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;

namespace WorkHunterUtils.Abstractions.HttpClients
{
    public interface ICurrencyClient
    {
        Task<CurrencyRate> GetCurrencies();

        Uri GetBaseAdrress();
    }
}
