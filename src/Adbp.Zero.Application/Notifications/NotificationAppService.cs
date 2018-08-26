using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Notifications;
using Abp.Runtime.Session;
using Adbp.Linq;
using Adbp.Paging.Dto;
using Adbp.Zero.Notifications.Dto;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero.Notifications
{
    public class NotificationAppService : ZeroAppServiceBase, INotificationAppService
    {
        private readonly IRepository<NotificationInfo, Guid> _notificationRepository;
        private readonly IRepository<TenantNotificationInfo, Guid> _tenantNotificationRepository;
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationRepository;
        private readonly IRepository<NotificationSubscriptionInfo, Guid> _notificationSubscriptionRepository;

        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly INotificationPublisher _notiticationPublisher;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly ILocalizationContext _localizationContext;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        public NotificationAppService(
            IRepository<NotificationInfo, Guid> notificationRepository,
            IRepository<TenantNotificationInfo, Guid> tenantNotificationRepository,
            IRepository<UserNotificationInfo, Guid> userNotificationRepository,
            IRepository<NotificationSubscriptionInfo, Guid> notificationSubscriptionRepository,

            INotificationDefinitionManager notificationDefinitionManager,
            INotificationPublisher notiticationPublisher,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IUserNotificationManager userNotificationManager,

            ILocalizationContext localizationContext,
            SysObjectSettingManager sysObjectSettingManager)
            : base(sysObjectSettingManager)
        {
            _notificationRepository = notificationRepository;
            _tenantNotificationRepository = tenantNotificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationSubscriptionRepository = notificationSubscriptionRepository;

            _notificationDefinitionManager = notificationDefinitionManager;
            _notiticationPublisher = notiticationPublisher;
            _userNotificationManager = userNotificationManager;
            _localizationContext = localizationContext;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }

        protected virtual UserIdentifier LoginIdentifier
            => new UserIdentifier(AbpSession.TenantId, AbpSession.GetUserId()); 

        public async Task SubscribeAsync(string notificationName)
        {
            await _notificationSubscriptionManager.SubscribeAsync(LoginIdentifier, notificationName);
        }

        public async Task UnsubscribeAsync(string notificationName)
        {
            await _notificationSubscriptionManager.UnsubscribeAsync(LoginIdentifier, notificationName);
        }

        public async Task<List<NotificationSubscriptionDto>> GetNotificationSubscriptionDtosAsync()
        {
            var list = await _notificationSubscriptionManager.GetSubscribedNotificationsAsync(LoginIdentifier);
            return list.Select(x => new NotificationSubscriptionDto
            {
                UserId = x.UserId,
                NotificationName = x.NotificationName,
                CreationTime = x.CreationTime
            }).ToList();
        }
        
        public async Task<List<NotificationDefinitionDto>> GetAllAvailableAsync()
        {
            var subscribes = (await _notificationSubscriptionManager.GetSubscribedNotificationsAsync(LoginIdentifier))
                .Where(x => x.EntityType == null).ToList();
            
            bool IsSubscribed(NotificationDefinition definition)
            {
                return subscribes.Any(x => x.NotificationName == definition.Name);
            }

            var list = await _notificationDefinitionManager.GetAllAvailableAsync(LoginIdentifier);
            return list.Select(x => new NotificationDefinitionDto
            {
                Name = x.Name,
                Description = x.DisplayName.Localize(_localizationContext),
                IsSubscribed = IsSubscribed(x)
            }).ToList();
        }

        public async Task<List<UserNotificationDto>> GetUserNotificationsAsync()
        {
            var definitions = _notificationDefinitionManager.GetAll();
            //var notificationDescription = definitions.Any(x=>x.Name == )
            string GetDescription(string notificationName)
            {
                return definitions.FirstOrDefault(x => x.Name == notificationName)?.DisplayName.Localize(_localizationContext) ?? notificationName;
            }

            var models = await _userNotificationManager.GetUserNotificationsAsync(LoginIdentifier, null);
            var list = models.Select(x => new UserNotificationDto
            {
                Id = x.Id,
                State = x.State,
                NotificationName = x.Notification.NotificationName,
                Description = GetDescription(x.Notification.NotificationName),
                Severity = x.Notification.Severity,
                CreationTime = x.Notification.CreationTime,
                EntityId = x.Notification.EntityId,
                EntityTypeName = x.Notification.EntityTypeName
            }).ToList();
            return list;
        }

        public async Task ReadNotificationAsync(Guid id)
        {
            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, id, UserNotificationState.Read);
        }

        public async Task UnreadNotificationAsync(Guid id)
        {
            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, id, UserNotificationState.Unread);
        }

        public async Task ReadNotificationsAsync(List<Guid> list)
        {
            foreach (var item in list)
            {
                await ReadNotificationAsync(item);
            }
        }

        public async Task UnreadNotificationsAsync(List<Guid> list)
        {
            foreach (var item in list)
            {
                await UnreadNotificationAsync(item);
            }
        }
    }
}
