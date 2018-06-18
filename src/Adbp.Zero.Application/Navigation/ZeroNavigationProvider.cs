using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Localization;
using Adbp.Zero.Authorization;

namespace Adbp.Zero.Navigation
{
    public class ZeroNavigationProvider : NavigationProvider
    {
        public static MenuItemDefinition AddItems(MenuItemDefinition definition, params MenuItemDefinition[] items)
        {
            foreach (var item in items)
            {
                definition = definition.AddItem(item);
            }
            return definition;
        }
        
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    AddItems(
                        new MenuItemDefinition("PersonalPortal", L("MENU_PersonalPortal"), icon: "menu"),
                        new MenuItemDefinition(PageNames.Home, L("MENU_HomePage"), url: "", icon: "home", requiredPermissionName: PermissionNames.Permissions_Home))
                 ).AddItem(
                    AddItems(new MenuItemDefinition("Administration", L("MENU_Administration"), icon: "menu"),
                        new MenuItemDefinition(PageNames.OrganizationUnit, L("MENU_OrganizationUnit"), url: "/zeroorganizationUnit/index", icon: "fork", requiredPermissionName: PermissionNames.Permissions_OrganizationUnit),
                        new MenuItemDefinition(PageNames.UserManagement, L("MENU_UserManagement"), url: "/zerousers/index", icon: "people", requiredPermissionName: PermissionNames.Permissions_User),
                        new MenuItemDefinition(PageNames.RoleManagement, L("MENU_RoleManagement"), url: "/zeroroles/index", icon: "target", requiredPermissionName: PermissionNames.Permissions_Role),

                        new MenuItemDefinition(PageNames.AuditLog, L("MENU_AuditLog"), url: "/zeroanalasy/auditLog", icon: "file", requiredPermissionName: PermissionNames.Permissions_AuditLog),
                        new MenuItemDefinition(PageNames.LoginAttemptLog, L("MENU_LoginAttemptLog"), url: "/zeroanalasy/loginAttemptLog", icon: "account-login", requiredPermissionName: PermissionNames.Permissions_LoginAttemptLog),

                        new MenuItemDefinition(PageNames.SystemSettings, L("MENU_SystemSettings"), url: "/zerosystem/settings", icon: "cog", requiredPermissionName: PermissionNames.Permissions_SystemSetting)
                    )
                 );
        }

        

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConsts.LocalizationSourceName);
        }
    }
}
