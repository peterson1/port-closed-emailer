using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Logging;
using PortClosedEmailer.Core.Configuration;

namespace PortClosedEmailer.Core.ViewModels
{
    public class HomeViewModel : ConfiguredViewModelBase
    {
        public HomeViewModel(AddNewHostViewModel addNewHostViewModel, IAppSettings appSettings, IMvxLogProvider mvxLogProvider) : base(appSettings, mvxLogProvider)
        {
            AddHostVM  = addNewHostViewModel ?? throw new ArgumentNullException();
            AddHostVM.HostInitialized += (s, e) => Hosts.Add(e);
        }


        public override async Task Initialize()
        {
            await base.Initialize();
            _cfg.HostsList.ForEach(_ => AddHostVM.AddHost(_));
            LogInfo("App initialized.");
        }


        public AddNewHostViewModel AddHostVM { get; }
        public ObservableCollection<HostScanViewModel> Hosts { get; } = new ObservableCollection<HostScanViewModel>();
    }
}
