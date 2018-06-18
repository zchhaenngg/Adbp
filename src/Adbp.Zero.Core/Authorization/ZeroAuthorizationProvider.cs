using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Adbp.Zero.Authorization
{
    public class ZeroAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Permissions_Tenant, L("Permissions_Tenant"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(PermissionNames.Permissions_User, L("Permissions_User"));
            context.CreatePermission(PermissionNames.Permissions_User_Create, L("Permissions_User_Create"));
            context.CreatePermission(PermissionNames.Permissions_User_Delete, L("Permissions_User_Delete"));
            context.CreatePermission(PermissionNames.Permissions_User_Retrieve, L("Permissions_User_Retrieve"));
            context.CreatePermission(PermissionNames.Permissions_User_Update, L("Permissions_User_Update"));

            context.CreatePermission(PermissionNames.Permissions_Role, L("Permissions_Role"));
            context.CreatePermission(PermissionNames.Permissions_Role_Create, L("Permissions_Role_Create"));
            context.CreatePermission(PermissionNames.Permissions_Role_Delete, L("Permissions_Role_Delete"));
            context.CreatePermission(PermissionNames.Permissions_Role_Retrieve, L("Permissions_Role_Retrieve"));
            context.CreatePermission(PermissionNames.Permissions_Role_Update, L("Permissions_Role_Update"));

            context.CreatePermission(PermissionNames.Permissions_OrganizationUnit, L("Permissions_OrganizationUnit"));
            context.CreatePermission(PermissionNames.Permissions_OrganizationUnit_Create, L("Permissions_OrganizationUnit_Create"));
            context.CreatePermission(PermissionNames.Permissions_OrganizationUnit_Delete, L("Permissions_OrganizationUnit_Delete"));
            context.CreatePermission(PermissionNames.Permissions_OrganizationUnit_Retrieve, L("Permissions_OrganizationUnit_Retrieve"));
            context.CreatePermission(PermissionNames.Permissions_OrganizationUnit_Update, L("Permissions_OrganizationUnit_Update"));

            context.CreatePermission(PermissionNames.Permissions_AuditLog, L("Permissions_AuditLog"));
            context.CreatePermission(PermissionNames.Permissions_LoginAttemptLog, L("Permissions_LoginAttemptLog"));
            context.CreatePermission(PermissionNames.Permissions_Home, L("Permissions_Home"));

            context.CreatePermission(PermissionNames.Permissions_SystemSetting, L("Permissions_SystemSetting"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConsts.LocalizationSourceName);
        }
    }
}
