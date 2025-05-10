using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunter.Models.Enums
{
    /// <summary>
    /// Типы задач в личном кабинете пользователя без учета иностранных вакансий
    /// </summary>
    public enum TaskType
    {
        [Description("Работодатель ответил, ожидается действие")]
        ReadTheEmployeeAnswer = 1,

        [Description("Заполнить анкету компании")]
        CompleteTheSurvey = 2,

        [Description("Пройти технический тест")]
        CompleteTheTest = 3,

        [Description("Выполнить техническое задание")]
        CompleteThechTask = 4,

        [Description("Пройти техническое интервью или звонок с представителями работодателя")]
        PassInterviewOrPhone = 5,

        [Description("Отправить личные документы представителям потенциального работодателя")]
        SendDocuments = 6
    }
}
