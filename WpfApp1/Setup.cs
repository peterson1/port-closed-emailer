using CommonLib.ExceptionTools;
using CommonLib.StringTools;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using PortClosedEmailer.Core.Configuration;
using System;
using System.IO;
using System.Windows;

namespace WpfApp1
{
    public class Setup : MvxWpfSetup
    {
        protected override IMvxApplication CreateApp()
        {
            var cfg = LoadConfig("appsettings.json");
            try
            {
                return new PortClosedEmailer.Core.App(cfg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Info(), "Setup.CreateApp");
                App.Current.Shutdown();
                throw ex;
            }
        }


        private IAppSettings LoadConfig(string jsonFilename)
        {
            //todo: handle missing config
            if (!File.Exists(jsonFilename))
                CreateDefaultSettingsFile(jsonFilename);
            try
            {
                var json = File.ReadAllText(jsonFilename);
                var cfg = JsonConvert.DeserializeObject<WpfAppSettings>(json);
                cfg.LoadExternalLists();
                cfg.SetDefaultValues();
                cfg.ValidateValues();
                return cfg;
            }
            catch (Exception ex)
            {
                var cap = $"Invalid {jsonFilename}";
                var msg = ex.Info(false, false) + L.F
                      + $"Please edit the file “{jsonFilename}”" + L.f
                      +  "and correct the invalid values.";
                MessageBox.Show(msg, cap);
                App.Current.Shutdown();
                return null;
            }
        }


        private void CreateDefaultSettingsFile(string jsonFilename)
        {
            var cfg = new WpfAppSettings();
            cfg.SetDefaultValues();
            var json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
            File.WriteAllText(jsonFilename, json);
        }
    }
}
