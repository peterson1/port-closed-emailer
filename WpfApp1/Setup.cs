using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using PortClosedEmailer.Core.Configuration;
using System;
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
            var json = File.ReadAllText(jsonFilename);
            return JsonConvert.DeserializeObject<WpfAppSettings>(json);
        }
    }
}
