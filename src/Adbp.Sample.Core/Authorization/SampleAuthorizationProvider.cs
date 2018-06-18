using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Localization;
using Adbp.Zero.Authorization;

namespace Adbp.Sample.Authorization
{
    public class SampleAuthorizationProvider: AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(SamplePermissionNames.Permissions_Guest, L("Permissions_Guest"));
            context.CreatePermission(SamplePermissionNames.Permissions_Guest_Create, L("Permissions_Guest_Create"));
            context.CreatePermission(SamplePermissionNames.Permissions_Guest_Update, L("Permissions_Guest_Update"));
            context.CreatePermission(SamplePermissionNames.Permissions_Guest_Delete, L("Permissions_Guest_Delete"));
            context.CreatePermission(SamplePermissionNames.Permissions_Guest_Retrieve, L("Permissions_Guest_Retrieve"));

            context.CreatePermission(SamplePermissionNames.Permissions_Contact, L("Permissions_Contact"));
            context.CreatePermission(SamplePermissionNames.Permissions_Contact_Create, L("Permissions_Guest_Create"));
            context.CreatePermission(SamplePermissionNames.Permissions_Contact_Update, L("Permissions_Guest_Update"));
            context.CreatePermission(SamplePermissionNames.Permissions_Contact_Delete, L("Permissions_Guest_Delete"));
            context.CreatePermission(SamplePermissionNames.Permissions_Contact_Retrieve, L("Permissions_Guest_Retrieve"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SampleConsts.LocalizationSourceName);
        }
    }
}
