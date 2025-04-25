using EmailSender.Constants.Notifications;
using EmailSender.Constants.Notifications.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;

namespace EmailSender
{
    public sealed class NotificationTagsHandler
    {
        private StringBuilder emailSubjectBuilder { get; set; }
        private StringBuilder emailBodyBuilder { get; set; }

        public NotificationTagsHandler(string emailSubjectBuilder, string emailBodyBuilder)
        {
            this.emailSubjectBuilder = new(emailSubjectBuilder);
            this.emailBodyBuilder = new(emailBodyBuilder);
        }

        public (string emailSubject, string emailBody) Build()
            => (emailSubjectBuilder.ToString(), emailBodyBuilder.ToString());

        public NotificationTagsHandler HandleCurrencyTags(List<string> vacancies, CurrencyRate currentRate)
        {
            this.emailBodyBuilder.Replace(CurrencyTags.PeriodDate, DateTime.UtcNow.ToString("yyyy/MM/dd"))
                            .Replace(CurrencyTags.VacancyList, string.Join(", ", vacancies));

            var dataTable = new StringBuilder();
            dataTable.Append(NotificationConstants.HtmlTableSettings);
            dataTable.Append("<tr><th width=\"150px\"> <b>Local currency</b></th> <th width=\"200px\"> <b>USD to local currency</b></th></tr>");
            GetBodyForCurrencyTable(currentRate, dataTable);
            dataTable.Append("</table>");

            emailBodyBuilder.Replace(CurrencyTags.CurrencyTableEn, dataTable.ToString());

            return this;
        }

        private void GetBodyForCurrencyTable(CurrencyRate currentRate, StringBuilder dataTable)
        {
            dataTable.Append("<tr><td>RUB</td><td>").Append(currentRate.conversion_rates.RUB).Append("</td></tr>");
            dataTable.Append("<tr><td>EUR</td><td>").Append(currentRate.conversion_rates.EUR).Append("</td></tr>");
            dataTable.Append("<tr><td>BR (byn)</td><td>").Append(currentRate.conversion_rates.BYN).Append("</td></tr>");
            dataTable.Append("<tr><td>SAR</td><td>").Append(currentRate.conversion_rates.SAR).Append("</td></tr>");
        }
    }
}
