using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunter.Models.Enums
{
    public enum UserTaskStatus
    {
        [Description("Требуется действие")]
        Open = 1,

        [Description("Завершена")]
        Closed = 2,
    }
}
