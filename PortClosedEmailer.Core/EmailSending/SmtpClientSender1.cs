using CommonLib.StringTools;
using PortClosedEmailer.Core.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.EmailSending
{
    public class SmtpClientSender1 : ConfiguredBase, IEmailSender
    {
        public event EventHandler<Exception> SendingError;


        public SmtpClientSender1(IAppSettings appSettings) : base(appSettings)
        {
        }


        public Task SendPortClosedAlert(string hostName)
        {
            var body = $"Failed to connect to host:{Environment.NewLine}{hostName}";
            return SendEmail($"Host down: {hostName}", body);
        }


        private async Task SendEmail(string subject, string body, int credentialsIndex = 0)
        {
            var creds   = _cfg.SmtpCredentials[credentialsIndex];
            var message = NewMailMessage(subject, body, creds);
            var smtp    = GetSmtpClient(creds);
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                SendingError?.Invoke(this, ex);
                await Task.Delay(1000 * 2);
                var nextIdx = (credentialsIndex == _cfg.SmtpCredentials.Count - 1)
                            ? 0 : credentialsIndex + 1;
                await SendEmail(subject, body, nextIdx);
            }
        }


        private MailMessage NewMailMessage(string subject, string body, (string Username, string Password) creds)
        {
            var sendr    = _cfg.SenderEmail.IsBlank() ? creds.Username : _cfg.SenderEmail;
            var fromAddr = new MailAddress(sendr, _cfg.SenderDisplayName);
            var toAddr   = new MailAddress(_cfg.RecipientEmail);
            return new MailMessage(fromAddr, toAddr)
            {
                Subject = subject,
                Body    = body
            };
        }


        private SmtpClient GetSmtpClient((string Username, string Password) creds) => new SmtpClient
        {
            Host                  = _cfg.SmtpHostName,
            Port                  = _cfg.SmtpPortNumber,
            EnableSsl             = _cfg.SmtpEnableSSL,
            DeliveryMethod        = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials           = new NetworkCredential(creds.Username, creds.Password)
        };
    }
}
