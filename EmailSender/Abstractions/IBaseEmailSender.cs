using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Abstractions
{
    public interface IBaseEmailSender
    {
        Task<bool> Send(string to, string subject, string body);
    }
}
