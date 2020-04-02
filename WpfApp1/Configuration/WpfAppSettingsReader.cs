using CommonLib.ExceptionTools;
using CommonLib.StringTools;
using Newtonsoft.Json;
using PortClosedEmailer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WpfApp1.WpfTools;

namespace WpfApp1.Configuration
{
    public partial class WpfAppSettings
    {
        private const string JSON_FILE = "appsettings.json";

        public static IAppSettings LoadJson()
        {
            //todo: handle missing config
            if (!File.Exists(JSON_FILE))
                CreateDefaultSettingsFile(JSON_FILE);
            try
            {
                var json = File.ReadAllText(JSON_FILE);
                var cfg = JsonConvert.DeserializeObject<WpfAppSettings>(json);
                cfg.LoadExternalLists();
                cfg.SetDefaultValues();
                cfg.ValidateValues();
                return cfg;
            }
            catch (Exception ex)
            {
                var msg = ex.Info(false, false) + L.F
                      + $"Please edit the file “{JSON_FILE}”" + L.f
                      +  "and correct the invalid values.";
                //MessageBox.Show(msg, $"Invalid {jsonFilename}");
                Alert.ShowModal($"Invalid {JSON_FILE}", msg);
                App.Current.Shutdown();
                return null;
            }
        }


        private static void CreateDefaultSettingsFile(string jsonFilename)
        {
            var cfg = new WpfAppSettings();
            cfg.SetDefaultValues();
            var json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
            File.WriteAllText(jsonFilename, json);
        }


        private void LoadExternalLists()
        {
            AppendHostsList();
            AppendCredentialsList();
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
