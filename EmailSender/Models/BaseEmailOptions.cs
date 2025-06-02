using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Models
{
    public class BaseEmailOptions
    {
        public required string Host { get; set; }

        public int Port { get; set; }

        public required string From { get; set; }

        public bool IsSSL { get; set; }

        public bool UseNtlmAuth { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public bool EmailSendingIsDisabled { get; set; }
    }
}
