using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailSender
    {
        // Synchronous send e-mail.
        void SendEmail(EmailMessage message);

        // Asynchronous send e-mail.
        Task SendEmailAsync(EmailMessage message);
    }
}
