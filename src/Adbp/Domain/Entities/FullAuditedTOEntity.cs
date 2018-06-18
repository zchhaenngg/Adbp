using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Adbp.Domain.Entities
{
    /// <summary>
    /// Full Audited Tentant Owner Entity
    /// </summary>
    public class FullAuditedTOEntity<TPrimaryKey, TUser> : FullAuditedEntity<TPrimaryKey, TUser>, IMustHaveTenant, IMayHaveOwner
        where TUser : IEntity<long>
    {
        public virtual int TenantId { get; set; }

        public virtual TUser Owner { get; set; }
        public virtual long? OwnerId { get; set; }
    }
}
