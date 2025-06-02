using EmailSender;
using EmailSender.Constants.Notifications;
using EmailSender.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkHunterUtils.Abstractions.Services;
using WorkHunterUtils.Models.ExternalApis.CurrencyApi;

namespace WorkHunterUtils.Services.Services.EmailSenders
{
    public class EmailService : BaseEmailSender<EmailOptions>, IEmailService
    {
        private readonly EmailOptions emailOptions;

        private readonly ILogger<EmailService> logger;

        public EmailService(IOptionsSnapshot<EmailOptions> emailOptions, ILogger<EmailService> logger) : base(emailOptions.Value, logger)
        {
            this.emailOptions = emailOptions.Value;
            this.logger = logger;
        }

        public async Task<bool> SendAboutVacancyCostChanged(string to, List<string> vacancies, CurrencyRate currentRate)
        {
            // var notification = await notificationService.GetByType(NotificationType.VacancyCostChanged)
            // if (notification.IsDeleted) continue;
            //NotificationTemplateConstants.VacancyCostChanged
            // var usersForNotification = await userService.GetUsersByRole();
            // emails = usersForNotification.Select(x => x.Email).ToList();
            // check was sent today
            var emailWithTags = new NotificationTagsHandler("", NotificationTemplateConstants.VacancyCostChanged);
            var (emailSubject, emailBody) = emailWithTags
                .HandleCurrencyTags(new List<string>() { "test1", "test2" }, currentRate)
                .Build();

            var isSent = await base.Send("olluntest@mail.ru", "Цена вакансий изменилась", emailBody);
            // TODO: log  if was sent

            return isSent;
        }
    }
}
