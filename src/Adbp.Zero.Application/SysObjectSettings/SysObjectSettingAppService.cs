using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Adbp.Paging.Dto;
using Adbp.Zero.Authorization;
using Adbp.Zero.SysObjectSettings.Dto;

namespace Adbp.Zero.SysObjectSettings
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_SysObjectSetting)]
    public class SysObjectSettingAppService : ZeroAppServiceBase, ISysObjectSettingAppService
    {
        private readonly IRepository<SysObjectSetting, long> _sysObjectSettingRepository;
        private readonly SysObjectSettingManager _sysObjectSettingManager;

        public SysObjectSettingAppService(
            IRepository<SysObjectSetting, long> sysObjectSettingRepository,
            SysObjectSettingManager sysObjectSettingManager) 
            : base(sysObjectSettingManager)
        {
            _sysObjectSettingRepository = sysObjectSettingRepository;
            _sysObjectSettingManager = sysObjectSettingManager;
        }

        //protected virtual async Task InsertOrUpdateAsync(SysObjectSetting entity)
        //{
        //    var trail = await _sysObjectSettingRepository.FirstOrDefaultAsync(x => x.SysObjectName == entity.SysObjectName && x.SysColumnName == entity.SysColumnName);
        //    if (trail != null)
        //    {
        //        trail = Map(entity, trail);
        //    }
        //    await _sysObjectSettingRepository.InsertOrUpdateAsync(trail);
        //}

        [AbpAuthorize(ZeroPermissionNames.Permissions_SysObjectSetting_Upsert)]
        public virtual async Task UpsertRoleSysObjectSettingAsync(RoleSysObjectSettingInput input)
        {
            var trail = await _sysObjectSettingRepository.FirstOrDefaultAsync(x => 
                x.SysObjectName == input.SysObjectName && x.RoleId == input.RoleId
            );
            if (trail == null)
            {
                trail = Map<SysObjectSetting>(input);
            }
            else
            {
                Map(input, trail);
            }
            await _sysObjectSettingRepository.InsertOrUpdateAsync(trail);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_SysObjectSetting_Delete)]
        public virtual async Task DeleteSysObjectSettingAsync(long accessControlId)
        {
            await _sysObjectSettingRepository.DeleteAsync(accessControlId);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_SysObjectSetting_Retrieve)]
        public virtual async Task<List<SysObjectSettingOutput>> GetSysObjectSettingOutputsForRoleAsync(int roleId)
        {
            var list = await _sysObjectSettingManager.GetSysObjectSettingInfosForRoleAsync(roleId);
            return list.Select(x => new SysObjectSettingOutput
            {
                SysObjectName = x.SysObjectName,
                AccessLevel = x.AccessLevel
            }).ToList();
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_SysObjectSetting_Retrieve)]
        public virtual async Task<List<SysColumnSettingOutput>> GetSysColumnSettingOutputsForRoleAsync(int roleId, string sysObjectName)
        {
            var list = await _sysObjectSettingManager.GetSysColumnSettingInfosForRoleAsync(roleId, sysObjectName);
            return list.Select(x => new SysColumnSettingOutput
            {
                SysObjectName = x.SysObjectName,
                SysColumnName = x.SysColumnName,
                AccessLevel = x.AccessLevel
            }).ToList();
        }
    }
}
