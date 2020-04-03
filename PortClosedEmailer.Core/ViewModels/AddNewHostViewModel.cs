using System;
using System.Threading.Tasks;
using CommonLib.StringTools;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using PortClosedEmailer.Core.Configuration;

namespace PortClosedEmailer.Core.ViewModels
{
    public class AddNewHostViewModel : ConfiguredViewModelBase
    {
        public event EventHandler<HostScanViewModel> HostInitialized;

        public AddNewHostViewModel(IAppSettings appSettings, IMvxLogProvider mvxLogProvider) : base(appSettings, mvxLogProvider)
        {
            AddHostCmd = new MvxCommand(async () => await AddNewHostAndSaveCfg());
        }


        public string       NewHostName  { get; set; }
        public IMvxCommand  AddHostCmd   { get; }


        private Task AddNewHostAndSaveCfg()
        {
            if (NewHostName.IsBlank()) return Task.FromResult(0);
            AddHost(NewHostName);
            _cfg.HostsList.Add(NewHostName);
            NewHostName = "";
            return _cfg.SaveCurrentValues();
        }


        public void AddHost(string host)
        {
            if (host.IsBlank() || host.StartsWith("//")) return;
            var vm = Mvx.IoCProvider.Resolve<HostScanViewModel>();
            vm.HostName = host;
            HostInitialized?.Invoke(this, vm);
            vm.StartScanCmd.Execute();
        }
    }
}
