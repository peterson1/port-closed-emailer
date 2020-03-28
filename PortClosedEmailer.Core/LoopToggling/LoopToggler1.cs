using PortClosedEmailer.Core.Configuration;
using PortClosedEmailer.Core.ScannerLooping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.LoopToggling
{
    public class LoopToggler1 : ConfiguredBase, ILoopToggler
    {
        public event EventHandler<string> FoundOpen;
        public event EventHandler<string> FoundClosed;

        private List<Task> _loops;
        private List<CancellationTokenSource> _cancels;

        private readonly IScannerLooper _loopr;


        public LoopToggler1(IScannerLooper scannerLooper,
                            IAppSettings appSettings) : base(appSettings)
        {
            _loopr = scannerLooper ?? throw new ArgumentNullException();
            _loopr.FoundOpen   += (s, e) => FoundOpen  ?.Invoke(this, e);
            _loopr.FoundClosed += (s, e) => FoundClosed?.Invoke(this, e);
        }


        public void StartAllLoops()
        {
            _cancels  = _cfg.HostsList.Select(_ => new CancellationTokenSource()).ToList();
            _loops    = _cfg.HostsList.Select((_, i) => _loopr.StartScanning(_, _cancels[i].Token)).ToList();
            Task.Run(() => Task.WhenAll(_loops));
        }


        public void StopOneLoop(int loopIndex)
        {
            _cancels[loopIndex].Cancel();
        }
    }
}
