using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Localization;
using Adbp.Zero.Authorization;

namespace CompanyName.ProjectName.Authorization
{
    public class ProjectNameAuthorizationProvider: AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(ProjectNamePermissionNames.Permissions_Guest, L("Permissions_Guest"));
            context.CreatePermission(ProjectNamePermissionNames.Permissions_Guest_Create, L("Permissions_Guest_Create"));
            context.CreatePermission(ProjectNamePermissionNames.Permissions_Guest_Update, L("Permissions_Guest_Update"));
            context.CreatePermission(ProjectNamePermissionNames.Permissions_Guest_Delete, L("Permissions_Guest_Delete"));
            context.CreatePermission(ProjectNamePermissionNames.Permissions_Guest_Retrieve, L("Permissions_Guest_Retrieve"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectNameConsts.LocalizationSourceName);
        }
    }
}
