using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Adbp.Zero.OrganizationUnits.Dto
{
    public class OrganizationUnitUserDto : CreationAuditedEntity<long>
    {
        public long OrganizationUnitId { get; set; }
        public string OrganizationUnitName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DisplayName { get; set; }
        public bool? IsStatic { get; set; }
    }
}
