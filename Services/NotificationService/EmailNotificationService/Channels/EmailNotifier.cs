using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace EmailNotificationService.Channels
{
    public class EmailNotifier : IEmailNotifier
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _userName;
        private readonly string _password;
        public EmailNotifier(string smtpServer, int smtpPort, string userName, string password)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _userName = userName;
            _password = password;
        }
        public async Task SendEmailAsync(string to, string from, string subject, string body)
        {
            using (SmtpClient client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_userName, _password);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(from);
                message.To.Add(to);
                message.Body = body;
                message.Subject = subject;

                await Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, r => TimeSpan.FromSeconds(2), (ex, ts) => { Console.WriteLine("Error sending email, retrying in 2 seconds"); })
                    .Execute(() => client.SendMailAsync(message))
                    .ContinueWith(_ => Console.WriteLine($"Notification email sent to: {to}"));
            }
        }
    }
}
