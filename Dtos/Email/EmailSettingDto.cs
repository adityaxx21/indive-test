using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indive_test.Dtos.Email
{
    public class EmailSettingDto
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}