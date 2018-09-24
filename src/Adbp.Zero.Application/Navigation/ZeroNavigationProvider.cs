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
        protected virtual MenuItemDefinition AddItems(MenuItemDefinition definition, params MenuItemDefinition[] items)
        {
            foreach (var item in items)
            {
                definition = definition.AddItem(item);
            }
            return definition;
        }

        protected virtual IEnumerable<MenuItemDefinition> GetPersonalPortalMenuItemDefinitions()
        {
            yield return new MenuItemDefinition(ZeroPageNames.Home, L("MENU_HomePage"), url: "", icon: "home", requiredPermissionName: ZeroPermissionNames.Permissions_Home);
        }

        protected virtual IEnumerable<MenuItemDefinition> GetAdministrationMenuItemDefinitions()
        {
            yield return new MenuItemDefinition(ZeroPageNames.OrganizationUnit, L("MENU_OrganizationUnit"), url: "/zeroorganizationUnit/index", icon: "fork", requiredPermissionName: ZeroPermissionNames.Permissions_OrganizationUnit);
            yield return new MenuItemDefinition(ZeroPageNames.UserManagement, L("MENU_UserManagement"), url: "/zerousers/index", icon: "people", requiredPermissionName: ZeroPermissionNames.Permissions_User);
            yield return new MenuItemDefinition(ZeroPageNames.RoleManagement, L("MENU_RoleManagement"), url: "/zeroroles/index", icon: "target", requiredPermissionName: ZeroPermissionNames.Permissions_Role);
            yield return new MenuItemDefinition(ZeroPageNames.Localization, L("MENU_Localization"), url: "/zeroLocalization/index", icon: "command", requiredPermissionName: ZeroPermissionNames.Permissions_ApplicationLanguageText);

            yield return new MenuItemDefinition(ZeroPageNames.AuditLog, L("MENU_AuditLog"), url: "/zeroanalasy/auditLog", icon: "file", requiredPermissionName: ZeroPermissionNames.Permissions_AuditLog);
            yield return new MenuItemDefinition(ZeroPageNames.LoginAttemptLog, L("MENU_LoginAttemptLog"), url: "/zeroanalasy/loginAttemptLog", icon: "account-login", requiredPermissionName: ZeroPermissionNames.Permissions_LoginAttemptLog);
            yield return new MenuItemDefinition(ZeroPageNames.SystemSettings, L("MENU_SystemSettings"), url: "/zerosystem/settings", icon: "cog", requiredPermissionName: ZeroPermissionNames.Permissions_SystemSetting);
        }

        protected virtual MenuItemDefinition GetAdministrationDefinition()
        {
            return AddItems(new MenuItemDefinition("Administration", L("MENU_Administration"), icon: "menu"),
                GetAdministrationMenuItemDefinitions().ToArray());
        }
        
        protected virtual MenuItemDefinition GetPersonalPortalDefinition()
        {
            return AddItems(new MenuItemDefinition("PersonalPortal", L("MENU_PersonalPortal"), icon: "menu"),
                GetPersonalPortalMenuItemDefinitions().ToArray());
        }

        protected virtual IEnumerable<MenuItemDefinition> GetMainMenuDefinitions()
        {
            yield return GetPersonalPortalDefinition();
            yield return GetAdministrationDefinition();
        }

        public override void SetNavigation(INavigationProviderContext context)
        {
            foreach (var item in GetMainMenuDefinitions().ToList())
            {
                context.Manager.MainMenu.AddItem(item);
            }
        }
        
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConsts.LocalizationSourceName);
        }
    }
}
