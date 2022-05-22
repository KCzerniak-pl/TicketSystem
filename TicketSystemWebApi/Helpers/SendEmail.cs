using EmailService;
using System.Text.RegularExpressions;

namespace TicketSystemWebApi.Helpers
{
    public class SendEmail
    {
        public async static void Send(IEmailSender _emailSender, int template, Database.Entities.Ticket ticket, Database.Entities.User? user = null)
        {
            // Dictionary for replace substring - variables.
            Dictionary<string, string> replacements = new Dictionary<string, string>
            {
                { "{ticket.No}", $"{ticket.No}" },
                { "{ticket.TicketID}", $"{ticket.TicketID}" },
                { "{ticket.DateTimeCreated}", $"{ticket.DateTimeCreated.ToString("dd/MM/yyyy HH:mm:ss")}" },
                { "{ticket.Owner.FirstName}", $"{ticket.Owner.FirstName}" },
                { "{ticket.Owner.LastName}", $"{ticket.Owner.LastName}" },
                { "{ticket.Title}", $"{ticket.Title}" },
                { "{ticket.Status.Name}", $"{ticket.Status.Name}" },
                { "{user.FirstName}", $"{user?.FirstName}" },
                { "{user.LastName}", $"{user?.LastName}" }
            };

            // Path to HTML template.
            string path = Directory.GetCurrentDirectory() + "\\Helpers\\EmailsTempates\\";
            if (template == 1) { path = path + "EmailTicketNew.txt"; }
            else if (template == 2) { path = path + "EmailStatusUpdate.txt"; }

            // Check if file exists (HTML template).
            if (System.IO.File.Exists(path))
            {
                // Read the file as one string - HTMl template.
                string content;
                using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.UTF8))
                {
                    content = streamReader.ReadToEnd();
                }

                // Replace substring.
                content = replacements.Aggregate(content, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

                // Get title from HTML template.
                string title = Regex.Match(content, "(?<=<title>)(.*)(?=</title>)").ToString();

                // Send e-mail.
                EmailMessage message = new EmailMessage(new string[] { ticket.Owner.Email }, title, content);
                await _emailSender.SendEmailAsync(message);
            }
        }
    }
}
