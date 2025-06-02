using Common.Constants.Notifications;
using EmailSender.Constants.Notifications.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Constants.Notifications
{
    public static class NotificationTemplateConstants
    {
        public const string VacancyCostChanged = $@"Уважаемый пользователь!{HtmlConst.Br}{HtmlConst.Br}
Курсы валют существенно изменились.{HtmlConst.Br} 
Отклики на вакансии с заработной платой в валюте: {CurrencyTags.VacancyList} {HtmlConst.Br}
Текущий курс за {CurrencyTags.PeriodDate}:{HtmlConst.Br}
{CurrencyTags.CurrencyTableEn}{HtmlConst.Br}
Для уточнения информации или при возникновении дополнительных вопросов, просьба обращаться по данному почтовому адресу .{HtmlConst.Br}
С уважением, WorkHunterHelper
{HtmlConst.Br}{HtmlConst.Br}
{HtmlConst.Hr}
{HtmlConst.Br}{HtmlConst.Br}
Dear User!{HtmlConst.Br}{HtmlConst.Br}
Salaries were changed.{HtmlConst.Br} 
Current currencies for {CurrencyTags.PeriodDate}:{HtmlConst.Br}
{CurrencyTags.CurrencyTableEn}{HtmlConst.Br}
Your requests on vacancies: {CurrencyTags.VacancyList}{HtmlConst.Br} 
To clarify the status of the request or if you have any additional questions, please contact us this email: .";
    }
}
