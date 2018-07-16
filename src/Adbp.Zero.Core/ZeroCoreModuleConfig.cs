using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration.Startup;

namespace Adbp.Zero
{
    public class ZeroCoreModuleConfig
    {
        public bool EnableZeroLdapAuthenticationSource { get; set; } = true;
    }

    public static class ZeroCoreModuleConfigExtensions
    {
        public static ZeroCoreModuleConfig ZeroCoreModule(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<ZeroCoreModuleConfig>();
        }
    }
}
