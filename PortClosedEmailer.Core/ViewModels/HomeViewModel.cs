using MvvmCross.ViewModels;
using PortClosedEmailer.Core.Configuration;
using PortClosedEmailer.Core.LoopToggling;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PortClosedEmailer.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly ILoopToggler _togl;
        private readonly IAppSettings _cfg;


        public HomeViewModel(ILoopToggler loopToggler, IAppSettings appSettings)
        {
            _togl = loopToggler ?? throw new ArgumentNullException();
            _cfg  = appSettings ?? throw new ArgumentNullException();
        }


        public override async Task Initialize()
        {
            await base.Initialize();
            _cfg.HostsList.ForEach(_ => HostNames.Add(_));
        }

        public ObservableCollection<string> HostNames { get; } = new ObservableCollection<string>();
    }
}
