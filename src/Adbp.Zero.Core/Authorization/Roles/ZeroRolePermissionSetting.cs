using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Roles;

namespace Adbp.Zero.Authorization.Roles
{
    public class ZeroRolePermissionSetting: RolePermissionSetting
    {
        /// <summary>
        /// Static RolePermissionSetting can not be deleted.
        /// </summary>
        public virtual bool IsStatic { get; set; }
    }
}
