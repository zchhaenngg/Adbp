using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using Adbp.Zero.Authorization;

namespace Adbp.Zero.Notifications
{
    public class ZeroNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    ZeroNotificationNames.NewUserRegistered,
                    displayName: L("NotificationDefinition_NewUserRegistered"),
                    permissionDependency: new SimplePermissionDependency(ZeroPermissionNames.Permissions_Notification_NewUserRegistered)
                )
            );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZeroConsts.LocalizationSourceName);
        }
    }
}
