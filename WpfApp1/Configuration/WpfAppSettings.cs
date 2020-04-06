using System.Collections.Generic;
using PortClosedEmailer.Core.Configuration;

namespace WpfApp1.Configuration
{
    public partial class WpfAppSettings : IAppSettings
    {
        public string   RecipientEmail       { get; set; }
        public string   SenderDisplayName    { get; set; }
        public string   SmtpHostName         { get; set; }
        public string   SmtpCredentialsFile  { get; set; }
        public string   HostsListFile        { get; set; }

        //optionals
        public string   SenderEmail          { get; set; }
        public int?     SmtpPortNumber       { get; set; }
        public bool?    SmtpEnableSSL        { get; set; }
        public int?     LoopDelaySeconds     { get; set; }


        public List<string> HostsList { get; set; }
        public List<SmtpCredential> SmtpCredentials { get; set; }



        public void SetDefaultValues()
        {
            RecipientEmail      = RecipientEmail      ?? "";
            SenderEmail         = SenderEmail         ?? "port@closed.emailer";
            SenderDisplayName   = SenderDisplayName   ?? "Port-closed Emailer";
            SmtpHostName        = SmtpHostName        ?? "";
            SmtpCredentialsFile = SmtpCredentialsFile ?? "";
            HostsListFile       = HostsListFile       ?? "";

            HostsList = HostsList ?? new List<string> { "google.com:80" };
            SmtpCredentials = SmtpCredentials ?? new List<SmtpCredential>
            {
                new SmtpCredential
                {
                    Username = "sample_username",
                    Password = "sample password"
                }
            };

            SmtpPortNumber   = SmtpPortNumber   ?? 587;
            SmtpEnableSSL    = SmtpEnableSSL    ?? true;
            LoopDelaySeconds = LoopDelaySeconds ?? 2;
        }
    }
}
