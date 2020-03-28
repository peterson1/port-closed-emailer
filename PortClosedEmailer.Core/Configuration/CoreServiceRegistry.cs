using Microsoft.Extensions.DependencyInjection;
using PortClosedEmailer.Core.EmailSending;
using PortClosedEmailer.Core.LoopToggling;
using PortClosedEmailer.Core.PortScanning;
using PortClosedEmailer.Core.ScannerLooping;

namespace PortClosedEmailer.Core.Configuration
{
    public static class CoreServiceRegistry
    {
        public static void RegisterCoreServices(this ServiceCollection s)
        {
            s.AddSingleton<IEmailSender, SmtpClientSender1>();
            s.AddSingleton<IScannerLooper, ScannerLooper1>();
            s.AddSingleton<ILoopToggler, LoopToggler1>();

            s.AddTransient<IPortScanner, TcpClientPortScanner1>();
        }
    }
}
