using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Adbp.Paging.Dto;
using Adbp.Zero.SysObjectSettings.Dto;

namespace Adbp.Zero.SysObjectSettings
{
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

        protected virtual async Task InsertOrUpdateAsync(SysObjectSetting entity)
        {
            var trail = await _sysObjectSettingRepository.FirstOrDefaultAsync(x => x.SysObjectName == entity.SysObjectName && x.SysColumnName == entity.SysColumnName);
            if (trail != null)
            {
                entity = Map(trail, entity);
            }
            await _sysObjectSettingRepository.InsertOrUpdateAsync(entity);
        }
        

        public virtual async Task CreateRoleSysObjectSettingAsync(RoleSysObjectSettingInput input)
        {
            var entity = Map<SysObjectSetting>(input);
            entity.SysColumnName = string.IsNullOrWhiteSpace(entity.SysColumnName) ? null : entity.SysColumnName;
            await InsertOrUpdateAsync(entity);
        }

        public virtual async Task DeleteSysObjectSettingAsync(long accessControlId)
        {
            await _sysObjectSettingRepository.DeleteAsync(accessControlId);
        }

        public virtual async Task<List<SysObjectSettingOutput>> GetSysObjectSettingOutputsForRoleAsync(int roleId)
        {
            var list = await _sysObjectSettingManager.GetSysObjectSettingInfosForRoleAsync(roleId);
            return list.Select(x => new SysObjectSettingOutput
            {
                SysObjectName = x.SysObjectName,
                AccessLevel = x.AccessLevel
            }).ToList();
        }

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
