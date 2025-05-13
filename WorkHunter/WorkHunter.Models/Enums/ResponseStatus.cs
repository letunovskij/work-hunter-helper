using System.ComponentModel;

namespace WorkHunter.Models.Enums;

public enum ResponseStatus
{
    [Description("Отклик подан")]
    Open = 1,

    [Description("Отклик просмотрен работодателем")]
    InitiallyViewedByEmployee = 2,

    [Description("Отклик просмотрен")]
    InitiallyViewedByMe = 3,

    [Description("Первичное интервью")]
    PhoneCallPlanning = 4,

    [Description("Получен ответ по результатам интервью")]
    PhoneCallFinished = 5,

    [Description("Первый раунд технического интервью назначен")]
    FirstTechInterviewPlanning = 6,

    [Description("Получен ответ по результатам первого технического интервью")]
    FirstTechInterviewFinished = 7,

    [Description("Второй раунд технического интервью назначен")]
    SecondTechInterviewPlanning = 8,

    [Description("Получен ответ по результатам второго технического интервью")]
    SecondTechInterviewFinished = 9,

    [Description("Третий раунд технического интервью назначен")]
    ThirdTechInterviewPlanning = 10,

    [Description("Получен ответ по результатам третьего технического интервью")]
    ThirdTechInterviewFinished = 11,

    [Description("Назначено интервью с руководством")]
    DirectorsInterviewPlanning = 12,

    DirectorsInterviewFinished = 13,

    [Description("Требуется прибыть в офис")]
    NeedToVisitTheOffice = 14,

    [Description("Требуется отправить личные документы")]
    NeedSendDocuments = 15,

    [Description("Джоб оффер получен")]
    JobOfferReceived = 16,

    [Description("Ожидается действие")]
    NeedAnswer = 17,

    [Description("Получена должность")]
    Taken = 18,

    [Description("Отклик не актуален")]
    Archived = 19
}
