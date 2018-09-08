using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Domain.Entities;

namespace Adbp.Zero.Authorization.Users
{
    public class LoginAgent : FullAuditedTOEntity<long, User>
    {
        public virtual long AgentId { get; set; }
        public virtual User Agent { get; set; }

        public virtual long PrincipalId { get; set; }
        public virtual User Principal { get; set; }
    }
}
