using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Adbp.Zero.Authorization.Users;
using Microsoft.AspNet.Identity;

namespace Adbp.Zero.Authorization.Roles
{
    public class RoleManager : RoleManager<Role, User>
    {
        private readonly IRepository<Role, int> _roleRepository;

        public RoleManager(
            IRepository<ZeroRolePermissionSetting, long> rolePermissionSettingRepository,
            IRepository<Role, int> roleRepository,

            RoleStore store, 
            IPermissionManager permissionManager, 
            IRoleManagementConfig roleManagementConfig, 
            ICacheManager cacheManager, 
            IUnitOfWorkManager unitOfWorkManager) 
            : base(
                  rolePermissionSettingRepository,
                  store, 
                  permissionManager, 
                  roleManagementConfig, 
                  cacheManager, 
                  unitOfWorkManager)
        {
            _roleRepository = roleRepository;
        }

        public override async Task<IdentityResult> UpdateAsync(Role role)
        {
            if (role.IsStatic)
            {//static role name cannot be changed.
                role.Name = await NullAsyncQueryableExecuter.Instance.FirstOrDefaultAsync(_roleRepository.GetAll().Where(x => x.Id == role.Id).Select(x => x.Name));
            }
            return await base.UpdateAsync(role);
        }
    }

    public abstract class RoleManager<TRole, TUser> : AbpRoleManager<TRole, TUser>
        where TRole : Role<TUser>, new()
        where TUser : User<TUser>
    {
        private readonly IRepository<ZeroRolePermissionSetting, long> _zeroRolePermissionSettingRepository;

        public RoleManager(
            IRepository<ZeroRolePermissionSetting, long> zeroRolePermissionSettingRepository,

            AbpRoleStore<TRole, TUser> store, 
            IPermissionManager permissionManager, 
            IRoleManagementConfig roleManagementConfig, 
            ICacheManager cacheManager, 
            IUnitOfWorkManager unitOfWorkManager) 
            : base(store, 
                  permissionManager, 
                  roleManagementConfig, 
                  cacheManager, 
                  unitOfWorkManager)
        {
            _zeroRolePermissionSettingRepository = zeroRolePermissionSettingRepository;
        }

        /// <summary>
        /// ignore static role permission
        /// </summary>
        /// <param name="role"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public override async Task SetGrantedPermissionsAsync(TRole role, IEnumerable<Permission> permissions)
        {
            var staticRolePermissions = await _zeroRolePermissionSettingRepository.GetAllListAsync(p => p.RoleId == role.Id && p.IsStatic);

            var oldPermissions = await GetGrantedPermissionsAsync(role);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => newPermissions.All(np => p.Name != np.Name)))
            {
                if (staticRolePermissions.Any(x => x.Name == permission.Name))
                {//ignore static role permission
                    continue;
                }
                await ProhibitPermissionAsync(role, permission);
            }

            foreach (var permission in newPermissions.Where(p => oldPermissions.All(op=> p.Name != op.Name)))
            {
                await GrantPermissionAsync(role, permission);
            }
        }
    }
}
