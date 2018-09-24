using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Application.Navigation;
using Abp.Localization;
using Adbp.Zero;
using Adbp.Zero.Navigation;
using CompanyName.ProjectName.Authorization;

namespace CompanyName.ProjectName.Web.App_Start
{
    public class ProjectNameNavigationProvider : ZeroNavigationProvider
    {
        protected override IEnumerable<MenuItemDefinition> GetPersonalPortalMenuItemDefinitions()
        {
            foreach (var item in base.GetPersonalPortalMenuItemDefinitions())
            {
                yield return item;
            }
            yield return new MenuItemDefinition(ProjectNamePageNames.Guests, L("MENU_Guests"), url: "/guests/index", icon: "people", requiredPermissionName: ProjectNamePermissionNames.Permissions_Guest);
        }

        protected override IEnumerable<MenuItemDefinition> GetMainMenuDefinitions()
        {
            yield return GetPersonalPortalDefinition();
            yield return AddItems(new MenuItemDefinition("Developer", L("MENU_Developer"), icon: "menu"),
                 new MenuItemDefinition(ZeroPageNames.Dev, new LocalizableString("MENU_Dev", ZeroConsts.LocalizationSourceName), url: "/ZeroDev/index", icon: "code")
            );
            yield return GetAdministrationDefinition();
        }


        public override void SetNavigation(INavigationProviderContext context)
        {
            base.SetNavigation(context);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectNameConsts.LocalizationSourceName);
        }
    }

    
}