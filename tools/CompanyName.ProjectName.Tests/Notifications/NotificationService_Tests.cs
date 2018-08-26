using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Zero.Notifications;
using Xunit;

namespace CompanyName.ProjectName.Tests.Notifications
{
    public class NotificationService_Tests: ProjectNameTestBase
    {
        private readonly INotificationAppService _notificationService;

        public NotificationService_Tests()
        {
            _notificationService = Resolve<INotificationAppService>();
        }
        
        [Fact]
        public async Task GetAllAvailableAsync()
        {
            var list = await _notificationService.GetAllAvailableAsync();
            Assert.True(list.Count == 1);
        }

        [Fact]
        public async Task SubscribeAsync()
        {
           await _notificationService.SubscribeAsync(ZeroNotificationNames.NewUserRegistered);

            //如果notification 不是由Provider提供，则找不到
            var availiables = await _notificationService.GetAllAvailableAsync();
            Assert.True(availiables.Count == 1);

            var subs = await _notificationService.GetNotificationSubscriptionDtosAsync();
            Assert.True(subs.Count == 1);

            await _notificationService.UnsubscribeAsync(ZeroNotificationNames.NewUserRegistered);

            var sub2s = await _notificationService.GetNotificationSubscriptionDtosAsync();
            Assert.True(sub2s.Count == 0);
            //
            //var userNotifications = await _notificationService.GetUserNotificationsAsync();
            //Assert.True(userNotifications.TotalCount == 1);
        }
    }
}
