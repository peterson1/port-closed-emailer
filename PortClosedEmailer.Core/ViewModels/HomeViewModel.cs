using MvvmCross;
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
            _cfg  = appSettings ?? throw new ArgumentNullException();
        }


        public override async Task Initialize()
        {
            await base.Initialize();
            foreach (var host in _cfg.HostsList)
            {
                var vm = Mvx.IoCProvider.Resolve<HostScanViewModel>();
                vm.HostName = host;
                Hosts.Add(vm);
                vm.StartScanCmd.Execute();
            }
        }

        public ObservableCollection<HostScanViewModel> Hosts { get; } = new ObservableCollection<HostScanViewModel>();
    }
}
