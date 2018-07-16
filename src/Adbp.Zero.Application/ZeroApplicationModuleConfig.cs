using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration.Startup;

namespace Adbp.Zero
{
    public class ZeroApplicationModuleConfig
    {
        public bool IsEmailWorkerEnabled { get; set; }
    }

    public static class ZeroApplicationModuleConfigExtensions
    {
        public static ZeroApplicationModuleConfig ZeroApplicationModule(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<ZeroApplicationModuleConfig>();
        }
    }
}
