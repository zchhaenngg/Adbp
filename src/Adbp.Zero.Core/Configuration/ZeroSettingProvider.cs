using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Localization;

namespace Adbp.Zero.Configuration
{
    public class ZeroSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var settingGroups_DATETIME = new SettingDefinitionGroup(ZeroConsts.SettingGroups.SettingGroups_DATETIME, L(nameof(ZeroConsts.SettingGroups.SettingGroups_DATETIME)));

            yield return new SettingDefinition(ZeroSettingNames.DateFormatting, "yyyy-MM-dd", L("SettingNames_DateFormatting"),
                group: settingGroups_DATETIME,
                scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, 
                isVisibleToClients: true);
            yield return new SettingDefinition(ZeroSettingNames.TimeFormatting, "HH-mm-ss", L("SettingNames_TimeFormatting"), 
                group: settingGroups_DATETIME,
                scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, 
                isVisibleToClients: true);
            yield return new SettingDefinition(ZeroSettingNames.DateTimeFormatting, "yyyy-MM-dd HH:mm:ss", L("SettingNames_DateTimeFormatting"),
                group: settingGroups_DATETIME,
                scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                isVisibleToClients: true);

            yield return new SettingDefinition(ZeroSettingNames.BackgroundWorkers.EmailWorkerTimerPeriodSeconds, "30", L("SettingNames_EmailWorkerTimerPeriodSeconds"),
                scopes: SettingScopes.Application | SettingScopes.Tenant, isVisibleToClients:true);
        }
        
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConsts.LocalizationSourceName);
        }
    }
}
