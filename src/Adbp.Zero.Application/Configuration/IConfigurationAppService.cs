using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Adbp.Zero.Configuration.Dto;

namespace Adbp.Zero.Configuration
{
    public interface IConfigurationAppService : IApplicationService
    {
        Task ChangeSettingForUserAsync(SettingValueInput input);
        Task ChangeSettingForTenantAsync(SettingValueInput input);
        Task ChangeSettingForApplicationAsync(SettingValueInput input);

        List<SettingDefinitionOutput> GetAllSettingDefinitionsForApplication();

        List<SettingDefinitionOutput> GetAllSettingDefinitionsForTenant();
    }
}
