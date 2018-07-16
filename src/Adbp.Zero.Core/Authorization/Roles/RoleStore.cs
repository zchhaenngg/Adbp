using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        protected RoleStore(IRepository<TRole> roleRepository, 
            IRepository<UserRole, long> userRoleRepository, 
            IRepository<RolePermissionSetting, long> rolePermissionSettingRepository) 
            : base(roleRepository, userRoleRepository, rolePermissionSettingRepository)
        {
        }
    }
}
