using System;
using System.Threading;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.ScannerLooping
{
    public interface IScannerLooper
    {
        event EventHandler<string> FoundOpen;
        event EventHandler<string> FoundClosed;

        Task StartScanning (string hostNameAndPortNumber, CancellationToken cancelToken);
    }
}
