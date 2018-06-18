using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Organizations;

namespace Adbp.Zero.OrganizationUnits.Dto
{
    [AutoMap(typeof(OrganizationUnit))]
    public class OrganizationUnitOutput
    {
        public virtual long Id { get; set; }
        public virtual long? ParentId { get; set; }
        public virtual string Code { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual int MemberCount { get; set; }
    }
}
