using CommonLib.StringTools;
using PortClosedEmailer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfApp1
{
    public class WpfAppSettings : IAppSettings
    {
        public string   RecipientEmail       { get; set; }
        public string   SenderDisplayName    { get; set; }
        public string   SmtpHostName         { get; set; }
        public string   SmtpCredentialsFile  { get; set; }
        public string   HostsListFile        { get; set; }

        //optionals
        public string   SenderEmail          { get; set; }
        public int      SmtpPortNumber       { get; set; }
        public bool     SmtpEnableSSL        { get; set; }
        public int      LoopDelaySeconds     { get; set; }


        public List<string> HostsList { get; set; }
        public List<(string Username, string Password)> SmtpCredentials { get; set; }


        public WpfAppSettings LoadExternalLists()
        {
            AppendHostsList();
            AppendCredentialsList();
            return this;
        }


        public WpfAppSettings SetDefaultValues()
        {
            SenderEmail      = "";
            SmtpPortNumber   = 587;
            SmtpEnableSSL    = true;
            LoopDelaySeconds = 2;
            return this;
        }


        private void AppendHostsList()
        {
            if (HostsList == null) HostsList = new List<string>();
            if (HostsListFile.IsBlank()) return;
            HostsList.AddRange(File.ReadAllLines(HostsListFile));
        }


        private void AppendCredentialsList()
        {
            if (SmtpCredentials == null) SmtpCredentials = new List<(string Username, string Password)>();
            if (SmtpCredentialsFile.IsBlank()) return;
            var lines = File.ReadAllLines(SmtpCredentialsFile);
            SmtpCredentials.AddRange(lines.Select((_, i) => ParseCredentialLine(_, i)));
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
