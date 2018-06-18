using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.OrganizationUnits.Dto
{
    public class OrganizationUserOuput
    {
        public long UserId { get; set; }
        public long OrganizationUnitId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
