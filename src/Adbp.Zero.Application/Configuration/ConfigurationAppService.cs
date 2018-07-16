using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Adbp.Zero.SysObjectSettings;
using Adbp.Zero.Authorization;
using Adbp.Zero.Configuration.Dto;
using Abp.Configuration;
using Abp.Localization;
using Abp.Auditing;

namespace Adbp.Zero.Configuration
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_SystemSetting)]
    public class ConfigurationAppService : ZeroAppServiceBase, IConfigurationAppService
    {
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly ILocalizationContext _localizationContext;

        public ConfigurationAppService(
            ISettingDefinitionManager settingDefinitionManager,

            ILocalizationContext localizationContext,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _settingDefinitionManager = settingDefinitionManager;
            _localizationContext = localizationContext;
        }

        [DisableAuditing]
        public virtual async Task ChangeSettingForUserAsync(SettingValueInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), input.Name, input.Value);
        }

        [DisableAuditing]
        public virtual async Task ChangeSettingForTenantAsync(SettingValueInput input)
        {
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), input.Name, input.Value);
        }

        [DisableAuditing]
        public virtual async Task ChangeSettingForApplicationAsync(SettingValueInput input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(input.Name, input.Value);
        }

        protected virtual SettingDefinitionOutput Convert(SettingDefinition definition)
        {
            var output = new SettingDefinitionOutput
            {
                Name = definition.Name,
                DisplayName = definition.DisplayName.Localize(_localizationContext)
            };
            if (definition.Group != null)
            {
                output.GroupName = definition.Group.Name;
                output.GroupDisplay = definition.Group.DisplayName.Localize(_localizationContext);
            }
            else
            {
                if (definition.Name.StartsWith("Abp.Zero.Ldap."))
                {
                    output.GroupName = ZeroConsts.SettingGroups.SettingGroups_LDAP;
                    output.GroupDisplay = L(nameof(ZeroConsts.SettingGroups.SettingGroups_LDAP));
                }
                else if (output.Name.StartsWith("Abp.Net.Mail."))
                {
                    output.GroupName = ZeroConsts.SettingGroups.SettingGroups_Mail;
                    output.GroupDisplay = L(nameof(ZeroConsts.SettingGroups.SettingGroups_Mail));
                }
                else if (output.Name.StartsWith("Abp.Zero.UserManagement."))
                {
                    output.GroupName = ZeroConsts.SettingGroups.SettingGroups_UserManagement;
                    output.GroupDisplay = L(nameof(ZeroConsts.SettingGroups.SettingGroups_UserManagement));
                }
                else if (output.Name.StartsWith("Adbp.BackgroundWorkers."))
                {
                    output.GroupName = ZeroConsts.SettingGroups.SettingGroups_BackgroundWorkers;
                    output.GroupDisplay = L(nameof(ZeroConsts.SettingGroups.SettingGroups_BackgroundWorkers));
                }
                else if (output.Name == "Abp.Localization.DefaultLanguageName"
                    || output.Name == "Abp.Timing.TimeZone")
                {
                    output.GroupName = ZeroConsts.SettingGroups.SettingGroups_LanguageTimeZone;
                    output.GroupDisplay = L(nameof(ZeroConsts.SettingGroups.SettingGroups_LanguageTimeZone));
                }
                else
                {
                    output.GroupName = ZeroConsts.SettingGroups.SettingGroups_Others;
                    output.GroupDisplay = L(nameof(ZeroConsts.SettingGroups.SettingGroups_Others));
                }
            }
            return output;
        }

        public virtual List<SettingDefinitionOutput> GetAllSettingDefinitionsForApplication()
        {
            return _settingDefinitionManager.GetAllSettingDefinitions()
                    .Where(x => x.Scopes.HasFlag(SettingScopes.Application))
                    .Select(Convert)
                    .ToList();
        }

        public virtual List<SettingDefinitionOutput> GetAllSettingDefinitionsForTenant()
        {
            return _settingDefinitionManager.GetAllSettingDefinitions()
                    .Where(x => x.Scopes.HasFlag(SettingScopes.Tenant))
                    .Select(Convert)
                    .ToList();
        }
    }
}
