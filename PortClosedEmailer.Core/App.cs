using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using PortClosedEmailer.Core.Configuration;
using PortClosedEmailer.Core.EmailSending;
using PortClosedEmailer.Core.LoopToggling;
using PortClosedEmailer.Core.PortScanning;
using PortClosedEmailer.Core.ScannerLooping;
using PortClosedEmailer.Core.ViewModels;

namespace PortClosedEmailer.Core
{
    public class App : MvxApplication
    {
        private IAppSettings _cfg;

        public App(IAppSettings appSettings)
        {
            _cfg = appSettings;
        }

        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IAppSettings>(_cfg);

            AddSingleton<IEmailSender, SmtpClientSender1>();
            AddSingleton<IScannerLooper, ScannerLooper1>();
            AddSingleton<ILoopToggler, LoopToggler1>();

            AddTransient<IPortScanner, TcpClientPortScanner1>();

            RegisterAppStart<HomeViewModel>();
        }


        private void AddSingleton<TInterface, TConcrete>()
            where TInterface : class
            where TConcrete  : class, TInterface
        {
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<TInterface, TConcrete>();
        }

        private void AddTransient<TInterface, TConcrete>()
            where TInterface : class
            where TConcrete  : class, TInterface
        {
            Mvx.IoCProvider.RegisterType<TInterface, TConcrete>();
        }
    }
}
