using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace Adbp.Zero.Authorization.Roles
{
    internal static class ZeroRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    ZeroStaticRoleNames.Host.Admin,
                    MultiTenancySides.Host)
                );

            //Static tenant roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    ZeroStaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant)
                );
        }
    }
}
