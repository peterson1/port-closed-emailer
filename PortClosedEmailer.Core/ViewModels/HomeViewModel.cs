using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PortClosedEmailer.Core.Configuration;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly IAppSettings _cfg;


        public HomeViewModel(IAppSettings appSettings)
        {
            _cfg = appSettings ?? throw new ArgumentNullException();
            AddHostCmd = new MvxCommand(() => AddHost(NewHostName));
        }


        public override async Task Initialize()
        {
            await base.Initialize();
            _cfg.HostsList.ForEach(_ => AddHost(_));
        }


        public ObservableCollection<HostScanViewModel> Hosts { get; } = new ObservableCollection<HostScanViewModel>();
        public IMvxCommand  AddHostCmd  { get; }
        public string NewHostName { get; set; }

        private void AddHost(string hostAndPort)
        {
            if (hostAndPort.StartsWith("//")) return;
            var vm = Mvx.IoCProvider.Resolve<HostScanViewModel>();
            vm.HostName = hostAndPort;
            Hosts.Add(vm);
            vm.StartScanCmd.Execute();
            NewHostName = "";
        }
    }
}
