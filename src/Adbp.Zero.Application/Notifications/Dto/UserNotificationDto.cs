using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Notifications;

namespace Adbp.Zero.Notifications.Dto
{
    public class UserNotificationDto : Entity<Guid>
    {
        public long UserId { get; set; }
        public UserNotificationState State { get; set; }
        public string NotificationName { get; set; }
        public string Description { get; set; }
        public NotificationSeverity Severity { get; set; }
        public DateTime CreationTime { get; set; }
        public string EntityTypeName { get; set; }
        public object EntityId { get; set; }

        public string StateStr => State.ToString();

        public string SeverityStr => Severity.ToString();
    }
}
