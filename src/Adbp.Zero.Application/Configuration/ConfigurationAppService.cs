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

namespace Adbp.Zero.Configuration
{
    [AbpAuthorize(PermissionNames.Permissions_SystemSetting)]
    public class ConfigurationAppService : ZeroAppServiceBase, IConfigurationAppService
    {
        public ConfigurationAppService(
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {

        }

        public virtual async Task ChangeUiDateAndTimeFormattingForApplicationAsync(ChangeUiDateAndTimeFormattingInput input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.UiDateFormatting, input.DateFormatting);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.UiTimeFormatting, input.TimeFormatting);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.UiDateTimeFormatting, input.DateTimeFormatting);
        }

        public virtual async Task ChangeUiDateAndTimeFormattingForTenantAsync(ChangeUiDateAndTimeFormattingInput input)
        {
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AppSettingNames.UiDateFormatting, input.DateFormatting);
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AppSettingNames.UiTimeFormatting, input.TimeFormatting);
            await SettingManager.ChangeSettingForTenantAsync(AbpSession.GetTenantId(), AppSettingNames.UiDateTimeFormatting, input.DateTimeFormatting);
        }

        public virtual async Task ChangeUiDateAndTimeFormattingForUserAsync(ChangeUiDateAndTimeFormattingInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiDateFormatting, input.DateFormatting);
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTimeFormatting, input.TimeFormatting);
            await  SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiDateTimeFormatting, input.DateTimeFormatting);
        }
    }
}
