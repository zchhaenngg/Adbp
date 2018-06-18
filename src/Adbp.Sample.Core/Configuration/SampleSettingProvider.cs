using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;

namespace Adbp.Sample.Configuration
{
    public class SampleSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            // return new[]
            // {
            //new SettingDefinition(AppSettingNames.UiDateFormatting, "yyyy-MM-dd", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            // };
            yield break;
        }
    }
}
