using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Abp.Runtime.Caching;
using Abp.Zero.Configuration;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Authorization.Roles
{
    public class RoleManager : AbpRoleManager<Role, User>
    {

        public RoleManager(
            RoleStore store, 
            IPermissionManager permissionManager, 
            IRoleManagementConfig roleManagementConfig, 
            ICacheManager cacheManager, 
            IUnitOfWorkManager unitOfWorkManager) 
            : base(
                  store, 
                  permissionManager, 
                  roleManagementConfig, 
                  cacheManager, 
                  unitOfWorkManager)
        {

        }
        
    }
}
