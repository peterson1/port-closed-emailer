using CommonLib.StringTools;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PortClosedEmailer.Core.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.ViewModels
{
    public class HomeViewModel : ConfiguredViewModelBase
    {
        public HomeViewModel(IAppSettings appSettings, IMvxLogProvider mvxLogProvider) : base(appSettings, mvxLogProvider)
        {
            AddHostCmd = new MvxCommand(() => AddHost(NewHostName, true));
        }


        public override async Task Initialize()
        {
            await base.Initialize();
            _cfg.HostsList.ForEach(_ => AddHost(_, false));
            ShowNewHostField = false;
            LogInfo("App initialized.");
        }


        public ObservableCollection<HostScanViewModel> Hosts { get; } = new ObservableCollection<HostScanViewModel>();
        public IMvxCommand  AddHostCmd  { get; }
        public string NewHostName { get; set; }

        public bool ShowNewHostField { get; private set; }

        public void AddHost(string hostAndPort, bool saveHostToCfg)
        {
            ShowNewHostField = true;
            if (hostAndPort.IsBlank()) return;
            if (hostAndPort.StartsWith("//")) return;
            var vm = Mvx.IoCProvider.Resolve<HostScanViewModel>();
            vm.HostName = hostAndPort;
            Hosts.Add(vm);
            vm.StartScanCmd.Execute();
            NewHostName = "";
            ShowNewHostField = false;

            if (saveHostToCfg)
            {
                _cfg.HostsList.Add(hostAndPort);
                _cfg.SaveCurrentValues();
            }
        }
    }
}
