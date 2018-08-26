using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Notifications;
using Adbp.Paging.Dto;
using Adbp.Zero.Notifications.Dto;

namespace Adbp.Zero.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        Task SubscribeAsync(string notificationName);
        Task UnsubscribeAsync(string notificationName);
        Task<List<NotificationDefinitionDto>> GetAllAvailableAsync();
        Task<List<NotificationSubscriptionDto>> GetNotificationSubscriptionDtosAsync();
        Task<List<UserNotificationDto>> GetUserNotificationsAsync();
        Task ReadNotificationAsync(Guid id);
        Task ReadNotificationsAsync(List<Guid> list);
        Task UnreadNotificationAsync(Guid id);
        Task UnreadNotificationsAsync(List<Guid> list);
    }
}
