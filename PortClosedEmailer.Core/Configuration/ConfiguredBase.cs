using MvvmCross.Logging;
using System;

namespace PortClosedEmailer.Core.Configuration
{
    public abstract class ConfiguredBase
    {
        protected readonly IAppSettings _cfg;
        protected readonly IMvxLog      _log;


        public ConfiguredBase(IAppSettings appSettings, IMvxLogProvider mvxLogProvider)
        {
            _cfg = appSettings ?? throw new ArgumentNullException();
            _log = mvxLogProvider.GetLogFor(this.GetType());
        }
    }
}
