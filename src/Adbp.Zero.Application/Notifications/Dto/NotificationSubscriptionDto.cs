using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Notifications.Dto
{
    public class NotificationSubscriptionDto
    {
        public long UserId { get; set; }
        public string NotificationName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
