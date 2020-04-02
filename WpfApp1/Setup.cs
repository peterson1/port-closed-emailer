using CommonLib.ExceptionTools;
using MvvmCross.Logging;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.ViewModels;
using Serilog;
using System;
using System.Windows;
using WpfApp1.Configuration;
using WpfApp1.WpfTools;

namespace WpfApp1
{
    public class Setup : MvxWpfSetup
    {
        protected override IMvxApplication CreateApp()
        {
            //GlobalErrors.HandleAll();
            var cfg = WpfAppSettings.LoadJson();
            try
            {
                return new PortClosedEmailer.Core.App(cfg);
            }
            catch (Exception ex)
            {
                Alert.Show(ex, "Setup.CreateApp");
                App.Current.Shutdown();
                throw ex;
            }
        }


        public override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.Serilog;


        protected override IMvxLogProvider CreateLogProvider()
        {
            var fmt = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{class}.{method}] {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.File(@"logs\debug.log", 
                            rollingInterval: RollingInterval.Day, 
                            outputTemplate: fmt)
                    .CreateLogger();
            return base.CreateLogProvider();
        }
    }
}
