using MvvmCross.Logging;
using MvvmCross.ViewModels;
using Serilog.Context;
using System;
using System.Runtime.CompilerServices;

namespace PortClosedEmailer.Core.Configuration
{
    public abstract class ConfiguredViewModelBase : MvxViewModel
    {
        protected readonly IAppSettings _cfg;
        private   readonly IMvxLog      _log;
        private   readonly string       _typNme;


        public ConfiguredViewModelBase(IAppSettings appSettings, IMvxLogProvider mvxLogProvider)
        {
            _cfg    = appSettings ?? throw new ArgumentNullException();
            _typNme = this.GetType().Name;
            _log    = mvxLogProvider.GetLogFor(_typNme);
        }


        protected void LogInfo(string message, [CallerMemberName] string caller = null)
        {
            using (LogContext.PushProperty("class", _typNme))
            using (LogContext.PushProperty("method", caller))
            {
                _log.Info(message);
            }
        }
    }
}
