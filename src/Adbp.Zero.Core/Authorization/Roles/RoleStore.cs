using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Authorization.Roles
{
    public class RoleStore : RoleStore<Role, User>
    {
        public RoleStore(
            IRepository<Role> roleRepository, 
            IRepository<UserRole, long> userRoleRepository, 
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository) 
            : base(
                  roleRepository, 
                  userRoleRepository, 
                  rolePermissionSettingRepository)
        {
        }
    }

    public abstract class RoleStore<TRole, TUser> : AbpRoleStore<TRole, TUser>
        where TRole : Role<TUser>
        where TUser : User<TUser>
    {
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionSettingRepository;

        protected RoleStore(IRepository<TRole> roleRepository, 
            IRepository<UserRole, long> userRoleRepository, 
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository) 
            : base(roleRepository, userRoleRepository, rolePermissionSettingRepository)
        {
            _rolePermissionSettingRepository = rolePermissionSettingRepository;
        }

        public override async Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(role.Id, permissionGrant))
            {
                return;
            }

            await _rolePermissionSettingRepository.InsertAsync(
                new ZeroRolePermissionSetting
                {
                    TenantId = role.TenantId,
                    RoleId = role.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted,
                    IsStatic = false
                });
        }
    }
}
