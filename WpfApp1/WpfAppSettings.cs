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
        public int?     SmtpPortNumber       { get; set; }
        public bool?    SmtpEnableSSL        { get; set; }
        public int?     LoopDelaySeconds     { get; set; }


        public List<string> HostsList { get; set; }
        public List<SmtpCredential> SmtpCredentials { get; set; }


        public void LoadExternalLists()
        {
            AppendHostsList();
            AppendCredentialsList();
        }


        public void SetDefaultValues()
        {
            RecipientEmail      = RecipientEmail      ?? "";
            SenderDisplayName   = SenderDisplayName   ?? "";
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

            SenderEmail      = SenderEmail      ?? "";
            SmtpPortNumber   = SmtpPortNumber   ?? 587;
            SmtpEnableSSL    = SmtpEnableSSL    ?? true;
            LoopDelaySeconds = LoopDelaySeconds ?? 2;
        }


        private void AppendHostsList()
        {
            if (HostsList == null) HostsList = new List<string>();
            if (HostsListFile.IsBlank()) return;
            //todo: handle missing file
            HostsList.AddRange(File.ReadAllLines(HostsListFile));
        }


        private void AppendCredentialsList()
        {
            if (SmtpCredentials == null) SmtpCredentials = new List<SmtpCredential>();
            if (SmtpCredentialsFile.IsBlank()) return;
            //todo: handle missing file
            var lines = File.ReadAllLines(SmtpCredentialsFile);
            SmtpCredentials.AddRange(lines.Select((_, i) => ParseCredentialLine(_, i)));
        }
        

        private SmtpCredential ParseCredentialLine(string line, int i)
        {
            if (!line.Contains(":"))
                throw new ArgumentException($"[{i}] Username and password should be separated by “:”");

            var ss = line.Split(':');
            if (ss.Length != 2)
                throw new ArgumentException($"[{i}] Invalid credentials line format");

            return new SmtpCredential
            {
                Username = ss[0],
                Password = ss[1]
            };
        }
    }
}
