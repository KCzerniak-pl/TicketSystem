using EmailService;
using System.Text.RegularExpressions;

namespace TicketSystemWebApi.Helpers
{
    public class SendEmail
    {
        public async static void Send(IEmailSender _emailSender, int template, Database.Entities.Ticket ticket, int ordinalNumber, Database.Entities.User user)
        {
            // Dictionary for replace substring - variables.
            Dictionary<string, string> replacements = new Dictionary<string, string>
            {
                { "{ticket.No}", $"{ordinalNumber}" },
                { "{ticket.TicketID}", $"{ticket.TicketID}" },
                { "{ticket.DateTimeCreated}", $"{ticket.DateTimeCreated.ToString("dd/MM/yyyy HH:mm:ss")}" },
                { "{ticket.FirstName}", $"{user.FirstName}" },
                { "{ticket.LastName}", $"{user.LastName}" },
                { "{ticket.Title}", $"{ticket.Title}" }
            };

            // Path to HTML template.
            string path = Directory.GetCurrentDirectory() + "\\Helpers\\EmailsTempates\\";
            if (template == 1) { path = path + "EmailTicketNew.txt"; }

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
                EmailMessage message = new EmailMessage(new string[] { user.Email }, title, content);
                await _emailSender.SendEmailAsync(message);
            }
        }
    }
}
