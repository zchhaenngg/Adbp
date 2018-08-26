using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;

namespace Adbp.Zero.Authorization.Roles.Dto
{
    [AutoMapFrom(typeof(Permission))]
    public class PermissionDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string GroupName
        {
            get
            {
                if (Name == "Permissions.User" || Name.StartsWith("Permissions.User."))
                {
                    return "USER";
                }
                else if (Name.StartsWith("Permissions.Role"))
                {
                    return "Role";
                }
                else if (Name.StartsWith("Permissions.UserRole"))
                {
                    return "USER_ROLE";
                }
                else if (Name.StartsWith("Permissions.SysObjectSetting"))
                {
                    return "SYSOBJECT";
                }
                else if (Name.StartsWith("Permissions.OrganizationUnit") || Name.StartsWith("Permissions.OuUser"))
                {
                    return "OU_USER";
                }
                else if (Name.StartsWith(ZeroPermissionNames.Permissions_ApplicationLanguageText))
                {
                    return "LOCALIZATION";
                }
                else if (Name.StartsWith("Permissions.Notification."))
                {
                    return "NOTIFICATION";
                }
                else if (Name == "Permissions.SystemSetting" ||
                    Name == "Permissions.AuditLog" ||
                    Name == "Permissions.LoginAttemptLog")
                {
                    return "SETTING_LOG";
                }
                return null;
            }
        }
    }
}
