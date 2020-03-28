using CommonLib.ExceptionTools;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PortClosedEmailer.Core.ScannerLooping;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.ViewModels
{
    public class HostScanViewModel : MvxViewModel
    {
        private string _dots;

        private readonly IScannerLooper _scanr;
        private CancellationTokenSource _cancelSrc;

        public HostScanViewModel(IScannerLooper scannerLooper)
        {
            _scanr       = scannerLooper ?? throw new ArgumentNullException();
            _scanr.FoundOpen   += (s, e) => DisplayOpen();
            _scanr.FoundClosed += (s, e) => Status = "Closed";
            StartScanCmd = new MvxCommand(() => JobNotifier = CreateNotifier());
            StopScanCmd  = new MvxCommand(() => StopScanning());
        }


        public string   HostName  { get; set; }
        public string   Status    { get; private set; }
        public string   Error     { get; private set; }

        public IMvxCommand   StartScanCmd  { get; private set; }
        public IMvxCommand   StopScanCmd   { get; private set; }
        public MvxNotifyTask JobNotifier   { get; private set; }


        private MvxNotifyTask CreateNotifier()
        {
            return MvxNotifyTask.Create(() => StartScanning(), ex => OnException(ex));
        }


        private void DisplayOpen()
        {
            _dots += " . ";
            if (_dots.Length >= 15) _dots = " . ";
            Status = $"Open [{_dots}]";
        }


        private Task StartScanning()
        {
            _cancelSrc = new CancellationTokenSource();
            return _scanr.StartScanning(HostName, _cancelSrc.Token);
        }


        private void StopScanning()
        {
            _cancelSrc?.Cancel();
            Status = "";
        }


        private void OnException(Exception ex)
        {
            Error = ex.Info();
        }
    }
}
