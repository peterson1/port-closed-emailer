using System;

namespace PortClosedEmailer.Core.Configuration
{
    public abstract class ConfiguredBase
    {
        protected readonly AppSettings _cfg;


        public ConfiguredBase(AppSettings appSettings)
        {
            _cfg = appSettings ?? throw new ArgumentNullException();
        }
    }
}
