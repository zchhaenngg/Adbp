using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Application.Navigation;
using Abp.Localization;
using Adbp.Sample.Authorization;
using Adbp.Zero.Navigation;

namespace Adbp.Sample.Web.App_Start
{
    public class SampleNavigationProvider : ZeroNavigationProvider
    {
        protected override IEnumerable<MenuItemDefinition> GetPersonalPortalMenuItemDefinitions()
        {
            foreach (var item in base.GetPersonalPortalMenuItemDefinitions())
            {
                yield return item;
            }
            yield return new MenuItemDefinition(SamplePageNames.Guests, L("MENU_Guests"), url: "/guests/index", icon: "people", requiredPermissionName: SamplePermissionNames.Permissions_Guest);
        }

        protected override IEnumerable<MenuItemDefinition> GetMainMenuDefinitions()
        {
            yield return GetPersonalPortalDefinition();
            yield return AddItems(new MenuItemDefinition("Developer", L("MENU_Developer"), icon: "menu"),
                new MenuItemDefinition(SamplePageNames.Guests, L("MENU_Guests"), url: "/guests/index", icon: "people", requiredPermissionName: SamplePermissionNames.Permissions_Guest)
            );
            yield return GetAdministrationDefinition();
        }


        public override void SetNavigation(INavigationProviderContext context)
        {
            base.SetNavigation(context);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SampleConsts.LocalizationSourceName);
        }
    }

    public static class SamplePageNames
    {
        public const string Guests = "Guests";
    }
}