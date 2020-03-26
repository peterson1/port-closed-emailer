namespace PortClosedEmailer.Core.Configuration
{
    public class AppSettings
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
    }
}
