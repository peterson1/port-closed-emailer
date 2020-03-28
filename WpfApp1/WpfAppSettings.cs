using PortClosedEmailer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfApp1
{
    public class WpfAppSettings : IAppSettings
    {
        private List<string> _hosts;
        private List<(string Username, string Password)> _creds;

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


        public List<string> HostsList 
            => _hosts ?? (_hosts = LoadHostsList());


        public List<(string Username, string Password)> SmtpCredentials
            => _creds ?? (_creds = LoadCredentials());


        private List<string> LoadHostsList()
        {
            return File.ReadAllLines(HostsListFile).ToList();
        }


        private List<(string Username, string Password)> LoadCredentials()
        {
            var lines = File.ReadAllLines(SmtpCredentialsFile);
            return lines.Select((_, i) => ParseCredentialLine(_, i)).ToList();
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
