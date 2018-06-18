using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Authorization
{
    /// <summary>
    /// 没有此实现，则会使用NullPermissionChecker,会始终返回true.   fuck...
    /// </summary>
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
