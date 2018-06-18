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
        public override void SetNavigation(INavigationProviderContext context)
        {
            base.SetNavigation(context);

            var personalPortal = context.Manager.MainMenu.GetItemByName("PersonalPortal");
            AddItems(personalPortal,
                new MenuItemDefinition(SamplePageNames.Guests, L("MENU_Guests"), url: "/guests/index", icon: "people", requiredPermissionName: SamplePermissionNames.Permissions_Guest),
                new MenuItemDefinition(SamplePageNames.Contacts, L("MENU_Contacts"), url: "/contacts/index", icon: "home", requiredPermissionName: SamplePermissionNames.Permissions_Contact)
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SampleConsts.LocalizationSourceName);
        }
    }

    public static class SamplePageNames
    {
        public const string Guests = "Guests";
        public const string Contacts = "Contacts";
    }
}