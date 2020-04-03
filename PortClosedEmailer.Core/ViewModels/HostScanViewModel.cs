using System;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.ExceptionTools;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PortClosedEmailer.Core.ScannerLooping;

namespace PortClosedEmailer.Core.ViewModels
{
    public class HostScanViewModel : MvxViewModel
    {
        private readonly IScannerLooper _scanr;
        private CancellationTokenSource _cancelSrc;

        public HostScanViewModel(IScannerLooper scannerLooper)
        {
            _scanr = scannerLooper ?? throw new ArgumentNullException();
            _scanr.FoundOpen   += (s, e) => DisplayOpen();
            _scanr.FoundClosed += (s, e) => DisplayClosed();
            StartScanCmd = new MvxCommand(() => JobNotifier = CreateNotifier());
            StopScanCmd  = new MvxCommand(() => StopScanning());
        }


        public string   HostName   { get; set; }
        public bool?    IsOpen     { get; private set; }
        public int      StepCount  { get; private set; }
        public int      StepMax    { get; } = 4;
        public string   Error      { get; private set; }

        public IMvxCommand   StartScanCmd  { get; }
        public IMvxCommand   StopScanCmd   { get; }
        public MvxNotifyTask JobNotifier   { get; private set; }


        private MvxNotifyTask CreateNotifier()
        {
            return MvxNotifyTask.Create(() => StartScanning(), ex => OnException(ex));
        }


        private void DisplayOpen()
        {
            IsOpen = true;
            StepCount = StepCount >= StepMax ? 0 : ++StepCount;
        }


        private void DisplayClosed()
        {
            IsOpen = false;
        }


        private Task StartScanning()
        {
            _cancelSrc = new CancellationTokenSource();
            return _scanr.StartScanning(HostName, _cancelSrc.Token);
        }


        private void StopScanning()
        {
            _cancelSrc?.Cancel();
            IsOpen = null;
        }


        private void OnException(Exception ex)
        {
            Error = ex.Info();
        }
    }
}
