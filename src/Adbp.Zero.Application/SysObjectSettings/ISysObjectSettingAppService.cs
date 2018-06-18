using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Paging.Dto;
using Adbp.Zero.SysObjectSettings.Dto;

namespace Adbp.Zero.SysObjectSettings
{
    public interface ISysObjectSettingAppService : IApplicationService
    {
        Task CreateRoleSysObjectSettingAsync(RoleSysObjectSettingInput input);
        Task DeleteSysObjectSettingAsync(long sysObjectSettingId);
        Task<List<SysObjectSettingOutput>> GetSysObjectSettingOutputsForRoleAsync(int roleId);
        Task<List<SysColumnSettingOutput>> GetSysColumnSettingOutputsForRoleAsync(int roleId, string sysObjectName);
    }
}
