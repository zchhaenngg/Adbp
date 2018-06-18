using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Organizations;
using Adbp.Zero.Authorization.Roles;

namespace Adbp.Zero.SysObjectSettings
{
    public class SysObjectSettingManager : IDomainService, ITransientDependency
    {
        private readonly IRepository<SysObjectSetting, long> _sysObjectSettingRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IMetadataManager _metaData;

        public SysObjectSettingManager(
            IRepository<SysObjectSetting, long> sysObjectSettingRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<UserRole, long> userRoleRepository,
            IMetadataManager metaData
            )
        {
            _sysObjectSettingRepository = sysObjectSettingRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _userRoleRepository = userRoleRepository;
            _metaData = metaData;
        }

        protected virtual AccessLevel DefaultLevel => AccessLevel.Reject;
        
        protected virtual async Task<List<string>> GetSysObjectNamesAsync()
        {
            return await _metaData.GetSysObjectNamesAsync();
        }

        protected virtual async Task<List<string>> GetSysColumnNamesAsync(string sysObjectName)
        {
            return await _metaData.GetSysColumnNamesAsync(sysObjectName);
        }

        private List<SysColumnSettingInfo> MergeSettings(List<SysObjectSetting> settings)
        {
            return settings.GroupBy(x => new { x.SysObjectName, x.SysColumnName }).Select(x => new SysColumnSettingInfo
            {
                SysObjectName = x.Key.SysObjectName,
                SysColumnName = x.Key.SysColumnName,
                AccessLevel = x.Select(y => y.AccessLevel).Aggregate((result, next) => result | next)
            }).ToList();
        }

        /// <summary>
        /// 根据提供的sysObjectSettings，获取关于sysObjectName的完整配置
        /// </summary>
        /// <param name="sysObjectName"></param>
        /// <param name="sysObjectSettings">sysObjectName的所有配置如某个角色的/某个组织/某个用户的</param>
        /// <returns></returns>
        private async Task<List<SysColumnSettingInfo>> CombineAndTransformAsync(string sysObjectName, List<SysObjectSetting> sysObjectSettings)
        {
            var settings = MergeSettings(sysObjectSettings);

            var results = (await GetSysColumnNamesAsync(sysObjectName))
                .Select(col => settings.FirstOrDefault(x => x.SysObjectName == sysObjectName && x.SysColumnName == col) ??
                   new SysColumnSettingInfo { SysObjectName = sysObjectName, SysColumnName = col, AccessLevel = DefaultLevel }).ToList();

            results.Insert(0, settings.FirstOrDefault(x => x.SysObjectName == sysObjectName && (x.SysColumnName == string.Empty || x.SysColumnName == null)) ??
                new SysColumnSettingInfo { SysObjectName = sysObjectName, SysColumnName = null, AccessLevel = DefaultLevel });
            return results;
        }

        /// <summary>
        /// 包含了一挑SysObjectSettingInfo信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sysObjectName"></param>
        /// <returns></returns>
        protected virtual async Task<List<SysColumnSettingInfo>> GetUserSettingsForObjectAsync(long userId, string sysObjectName)
        {
            //因用户组
            var uorg_ac_queryable = from ac in _sysObjectSettingRepository.GetAll().Where(x => x.SysObjectName == sysObjectName && x.OrganizationUnitId != null)
                                    join uorg in _userOrganizationUnitRepository.GetAll().Where(x => x.UserId == userId) on ac.OrganizationUnitId equals uorg.OrganizationUnitId
                                    select ac;
            //因角色
            var urole_ac_queryable = from ac in _sysObjectSettingRepository.GetAll().Where(x => x.SysObjectName == sysObjectName && x.RoleId != null)
                                     join urole in _userRoleRepository.GetAll().Where(x => x.UserId == userId) on ac.RoleId equals urole.RoleId
                                     select ac;
            //用户可能会出现在多个组和多个角色中, 所以需要distinct
            var list = uorg_ac_queryable.Union(urole_ac_queryable).Distinct().ToList();

            return await CombineAndTransformAsync(sysObjectName, list);
        }

        public virtual async Task<List<SysColumnSettingInfo>> GetSysColumnSettingInfosForRoleAsync(int roleId, string sysObjectName)
        {
            var list = await _sysObjectSettingRepository.GetAllListAsync(x => x.SysObjectName == sysObjectName && x.RoleId == roleId);
            return await CombineAndTransformAsync(sysObjectName, list);
        }

        /// <summary>
        /// 1Role-*SysObjects
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public virtual async Task<List<SysObjectSettingInfo>> GetSysObjectSettingInfosForRoleAsync(int roleId)
        {
            var list = await _sysObjectSettingRepository.GetAllListAsync(x => x.RoleId == roleId && (x.SysColumnName == string.Empty || x.SysColumnName == null));
            return (await GetSysObjectNamesAsync())
                .Select(name => new SysObjectSettingInfo
                {
                    SysObjectName = name,
                    AccessLevel = list.FirstOrDefault(x => x.SysObjectName == name)?.AccessLevel ?? DefaultLevel,
                }).ToList();
        }
        

        /// <summary>
        /// sysObjectName的配置信息（组织）
        /// </summary>
        /// <param name="ouId"></param>
        /// <param name="sysObjectName"></param>
        /// <returns></returns>
        protected virtual async Task<List<SysColumnSettingInfo>> GetSysColumnSettingInfosForOuAsync(int ouId, string sysObjectName)
        {
            var list = await _sysObjectSettingRepository.GetAllListAsync(x => x.SysObjectName == sysObjectName && x.OrganizationUnitId == ouId);
            return await CombineAndTransformAsync(sysObjectName, list);
        }

        
        /// <summary>
        /// 用户对sysObjectName对应的数据源有读取权限吗
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sysObjectName"></param>
        /// <returns></returns>
        public virtual async Task<bool> HasRetrieveAccessLevelAsync(long userId, string sysObjectName)
        {
            var list = await GetUserSettingsForObjectAsync(userId, sysObjectName);
            return list.Single(x => string.IsNullOrWhiteSpace(x.SysColumnName)).AccessLevel.HasFlag(AccessLevel.Retrieve);
        }
        
    }
}
