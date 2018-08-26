using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Notifications.Dto
{
    public class NotificationDefinitionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsSubscribed { get; set; }
    }
}
