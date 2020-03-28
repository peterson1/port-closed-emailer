using System;

namespace PortClosedEmailer.Core.Configuration
{
    public abstract class ConfiguredBase
    {
        protected readonly IAppSettings _cfg;


        public ConfiguredBase(IAppSettings appSettings)
        {
            _cfg = appSettings ?? throw new ArgumentNullException();
        }
    }
}
