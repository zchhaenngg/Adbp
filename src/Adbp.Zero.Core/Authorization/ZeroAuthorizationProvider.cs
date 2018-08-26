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
            context.CreatePermission(ZeroPermissionNames.Permissions_Tenant, L("Permissions_Tenant"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(ZeroPermissionNames.Permissions_User, L("Permissions_User"));
            context.CreatePermission(ZeroPermissionNames.Permissions_User_Create, L("Permissions_User_Create"));
            context.CreatePermission(ZeroPermissionNames.Permissions_User_Delete, L("Permissions_User_Delete"));
            context.CreatePermission(ZeroPermissionNames.Permissions_User_Retrieve, L("Permissions_User_Retrieve"));
            context.CreatePermission(ZeroPermissionNames.Permissions_User_Update, L("Permissions_User_Update"));

            context.CreatePermission(ZeroPermissionNames.Permissions_UserAgent, L("Permissions_UserAgent"));
            context.CreatePermission(ZeroPermissionNames.Permissions_UserAgent_Upsert, L("Permissions_UserAgent_Upsert"));

            context.CreatePermission(ZeroPermissionNames.Permissions_Role, L("Permissions_Role"));
            context.CreatePermission(ZeroPermissionNames.Permissions_Role_Create, L("Permissions_Role_Create"));
            context.CreatePermission(ZeroPermissionNames.Permissions_Role_Delete, L("Permissions_Role_Delete"));
            context.CreatePermission(ZeroPermissionNames.Permissions_Role_Retrieve, L("Permissions_Role_Retrieve"));
            context.CreatePermission(ZeroPermissionNames.Permissions_Role_Update, L("Permissions_Role_Update"));

            context.CreatePermission(ZeroPermissionNames.Permissions_UserRole_Upsert, L("Permissions_UserRole_Upsert"));
            context.CreatePermission(ZeroPermissionNames.Permissions_UserRole_Retrieve, L("Permissions_UserRole_Retrieve"));

            context.CreatePermission(ZeroPermissionNames.Permissions_OrganizationUnit, L("Permissions_OrganizationUnit"));
            context.CreatePermission(ZeroPermissionNames.Permissions_OrganizationUnit_Create, L("Permissions_OrganizationUnit_Create"));
            context.CreatePermission(ZeroPermissionNames.Permissions_OrganizationUnit_Delete, L("Permissions_OrganizationUnit_Delete"));
            context.CreatePermission(ZeroPermissionNames.Permissions_OrganizationUnit_Retrieve, L("Permissions_OrganizationUnit_Retrieve"));
            context.CreatePermission(ZeroPermissionNames.Permissions_OrganizationUnit_Update, L("Permissions_OrganizationUnit_Update"));

            context.CreatePermission(ZeroPermissionNames.Permissions_OuUser_Create, L("Permissions_OuUser_Create"));
            context.CreatePermission(ZeroPermissionNames.Permissions_OuUser_Retrieve, L("Permissions_OuUser_Retrieve"));
            context.CreatePermission(ZeroPermissionNames.Permissions_OuUser_Delete, L("Permissions_OuUser_Delete"));

            context.CreatePermission(ZeroPermissionNames.Permissions_SysObjectSetting, L("Permissions_SysObjectSetting"));
            context.CreatePermission(ZeroPermissionNames.Permissions_SysObjectSetting_Retrieve, L("Permissions_SysObjectSetting_Retrieve"));
            context.CreatePermission(ZeroPermissionNames.Permissions_SysObjectSetting_Upsert, L("Permissions_SysObjectSetting_Upsert"));
            context.CreatePermission(ZeroPermissionNames.Permissions_SysObjectSetting_Delete, L("Permissions_SysObjectSetting_Delete"));

            context.CreatePermission(ZeroPermissionNames.Permissions_ApplicationLanguageText, L("Permissions_ApplicationLanguageText"));
            context.CreatePermission(ZeroPermissionNames.Permissions_ApplicationLanguageText_Retrieve, L("Permissions_ApplicationLanguageText_Retrieve"));
            context.CreatePermission(ZeroPermissionNames.Permissions_ApplicationLanguageText_Upsert, L("Permissions_ApplicationLanguageText_Upsert"));

            context.CreatePermission(ZeroPermissionNames.Permissions_AuditLog, L("Permissions_AuditLog"));
            context.CreatePermission(ZeroPermissionNames.Permissions_LoginAttemptLog, L("Permissions_LoginAttemptLog"));
            context.CreatePermission(ZeroPermissionNames.Permissions_Home, L("Permissions_Home"));

            context.CreatePermission(ZeroPermissionNames.Permissions_SystemSetting, L("Permissions_SystemSetting"));

            context.CreatePermission(ZeroPermissionNames.Permissions_Notification_NewUserRegistered, L("Permissions_Notification_NewUserRegistered"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConsts.LocalizationSourceName);
        }
    }
}
