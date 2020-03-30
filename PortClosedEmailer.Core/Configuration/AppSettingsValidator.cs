using CommonLib.StringTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PortClosedEmailer.Core.Configuration
{
    public static class AppSettingsValidator
    {
        public static void ValidateValues(this IAppSettings cfg)
        {
            MustNotBeBlank(cfg.RecipientEmail  , "RecipientEmail");
            MustNotBeBlank(cfg.SmtpHostName    , "SmtpHostName");
            MustNotBeEmpty(cfg.SmtpCredentials , "SmtpCredentials");
            MustNotBeNull (cfg.SmtpPortNumber  , "SmtpPortNumber");
            MustNotBeNull (cfg.SmtpEnableSSL   , "SmtpEnableSSL");
            MustNotBeNull (cfg.LoopDelaySeconds, "LoopDelaySeconds");
        }


        private static void MustNotBeBlank(string text, string desc)
        {
            if (text.IsBlank())
                throw new ArgumentException($"[{desc}] must NOT be blank.");
        }

        private static void MustNotBeNull<T>(Nullable<T> value, string desc) where T : struct
        {
            if (!value.HasValue)
                throw new ArgumentException($"[{desc}] must HAVE a value.");
        }

        private static void MustNotBeEmpty<T>(IEnumerable<T> list, string desc)
        {
            if (!list?.Any() ?? true)
                throw new ArgumentException($"[{desc}] must NOT be an empty list.");
        }
    }
}
