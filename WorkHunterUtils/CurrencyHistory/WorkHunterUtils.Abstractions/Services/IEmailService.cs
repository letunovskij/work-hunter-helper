using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;

namespace WorkHunterUtils.Abstractions.Services
{
    public interface IEmailService
    {
        Task<bool> SendAboutVacancyCostChanged(string to, List<string> vacancies, CurrencyRate currentRate);
    }
}
