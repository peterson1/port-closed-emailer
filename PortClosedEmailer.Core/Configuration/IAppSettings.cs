using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.Configuration
{
    public interface IAppSettings
    {
        string   RecipientEmail       { get; }
        string   SenderDisplayName    { get; }
        string   SmtpHostName         { get; }
        List<string>   HostsList      { get; }
        List<SmtpCredential>  SmtpCredentials  { get; }

        //optionals
        string   SenderEmail          { get; }
        int?     SmtpPortNumber       { get; }
        bool?    SmtpEnableSSL        { get; }
        int?     LoopDelaySeconds     { get; }

        Task     SaveCurrentValues    ();
    }


    public class SmtpCredential
    {
        public string  Username  { get; set; }
        public string  Password  { get; set; }
    }
}
