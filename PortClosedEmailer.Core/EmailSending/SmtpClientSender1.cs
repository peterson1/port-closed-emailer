using PortClosedEmailer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.EmailSending
{
    public class SmtpClientSender1 : ConfiguredBase, IEmailSender
    {
        public event EventHandler<Exception> SendingError;

        private List<(string Username, string Password)> _creds;


        public SmtpClientSender1(AppSettings appSettings) : base(appSettings)
        {
        }


        public Task SendPortClosedAlert(string hostName)
        {
            if (_creds == null) FillCredentials();
            var body = $"Failed to connect to host:{Environment.NewLine}{hostName}";
            return SendEmail($"Host down: {hostName}", body);
        }


        private async Task SendEmail(string subject, string body, int credentialsIndex = 0)
        {
            var message = NewMailMessage(subject, body);
            var smtp    = GetSmtpClient(_creds[credentialsIndex]);
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                SendingError?.Invoke(this, ex);
                await Task.Delay(1000 * 2);
                var nextIdx = (credentialsIndex == _creds.Count - 1)
                            ? 0 : credentialsIndex + 1;
                await SendEmail(subject, body, nextIdx);
            }
        }


        private MailMessage NewMailMessage(string subject, string body)
        {
            var fromAddr = new MailAddress(_cfg.SenderEmail, _cfg.SenderDisplayName);
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
            Port                  = _cfg.SmtpPortNumber ?? 587,
            EnableSsl             = _cfg.SmtpEnableSSL  ?? true,
            DeliveryMethod        = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials           = new NetworkCredential(creds.Username, creds.Password)
        };


        private void FillCredentials()
        {
            var lines = File.ReadAllLines(_cfg.SmtpCredentialsFile);
            _creds = lines.Select((_, i) => ParseCredentialLine(_, i)).ToList();
        }


        private (string Username, string Password) ParseCredentialLine(string line, int i)
        {
            if (!line.Contains(":"))
                throw new ArgumentException($"[{i}] Username and password should be separated by “:”");
            
            var ss = line.Split(':');
            if (ss.Length != 2)
                throw new ArgumentException($"[{i}] Invalid credentials line format");

            return (ss[0], ss[1]);
        }
    }
}
