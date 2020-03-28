using FluentAssertions;
using PortClosedEmailer.Core.Configuration;
using System;
using System.IO;
using WpfApp1;
using static System.Environment;

namespace Tests.Configuration
{
    public class TestSettings : WpfAppSettings
    {
        private const string DIR = "Port-closed Emailer files";

        public TestSettings()
        {
            RecipientEmail      = "petersonsalamat@gmail.com";
            SenderDisplayName   = "Test Sender";
            SmtpHostName        = "smtp.gmail.com";
            SmtpCredentialsFile = DesktopDir("creds.txt");
            HostsListFile       = DesktopDir("hosts.txt");
            //SenderEmail         = "test@send.er";
            //SmtpPortNumber      = 587;
            //SmtpEnableSSL       = true;
        }


        public TestSettings(string credsFilename) : this()
        {
            SmtpCredentialsFile = DesktopDir(credsFilename);
        }


        private string DesktopDir(string fileName)
        {
            var desktop = GetFolderPath(SpecialFolder.DesktopDirectory);
            var path = Path.Combine(desktop, DIR, fileName);
            File.Exists(path).Should().BeTrue();
            return path;
        }
    }
}
