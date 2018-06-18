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
    public class RoleStore : AbpRoleStore<Role, User>
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
}
