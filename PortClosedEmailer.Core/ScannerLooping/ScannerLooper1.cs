using PortClosedEmailer.Core.Configuration;
using PortClosedEmailer.Core.EmailSending;
using PortClosedEmailer.Core.PortScanning;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.ScannerLooping
{
    public class ScannerLooper1 : ConfiguredBase, IScannerLooper
    {
        public event EventHandler<string> FoundOpen;
        public event EventHandler<string> FoundClosed;

        private readonly IPortScanner _scanr;
        private readonly IEmailSender _emailr;

        public ScannerLooper1(IPortScanner portScanner,
                              IEmailSender emailSender,
                              IAppSettings appSettings) : base(appSettings)
        {
            _scanr  = portScanner ?? throw new ArgumentNullException();
            _emailr = emailSender ?? throw new ArgumentNullException();
        }


        public async Task StartScanning(string hostAndPort, CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                if (await _scanr.IsPortOpen(hostAndPort))
                    FoundOpen?.Invoke(this, hostAndPort);
                else
                {
                    FoundClosed?.Invoke(this, hostAndPort);
                    await _emailr.SendPortClosedAlert(hostAndPort);
                    return;
                }
                await Task.Delay(1000 * (_cfg.LoopDelaySeconds ?? 2));
            }
        }
    }
}
