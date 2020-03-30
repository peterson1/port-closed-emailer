using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using PortClosedEmailer.Core.Configuration;
using System.IO;

namespace WpfApp1
{
    public class Setup : MvxWpfSetup
    {
        protected override IMvxApplication CreateApp()
        {
            var cfg = LoadConfig("appsettings.json");
            return new PortClosedEmailer.Core.App(cfg);
        }


        private IAppSettings LoadConfig(string jsonFilename)
        {
            //todo: handle missing config
            var json = File.ReadAllText(jsonFilename);
            var cfg  = JsonConvert.DeserializeObject<WpfAppSettings>(json);
            return cfg.LoadExternalLists()
                      .SetDefaultValues();
        }
    }
}
