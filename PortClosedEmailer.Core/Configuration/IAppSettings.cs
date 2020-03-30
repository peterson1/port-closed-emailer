using System.Collections.Generic;

namespace PortClosedEmailer.Core.Configuration
{
    public interface IAppSettings
    {
        string   RecipientEmail       { get; }
        string   SenderDisplayName    { get; }
        string   SmtpHostName         { get; }
        List<string>   HostsList      { get; }
        List<(string Username, string Password)>  SmtpCredentials  { get; }

        //optionals
        string   SenderEmail          { get; }
        int      SmtpPortNumber       { get; }
        bool     SmtpEnableSSL        { get; }
        int      LoopDelaySeconds     { get; }
    }
}
