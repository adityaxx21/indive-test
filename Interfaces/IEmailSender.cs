using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indive_test.Interfaces
{
    public interface IEmailSender
    {
         Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}