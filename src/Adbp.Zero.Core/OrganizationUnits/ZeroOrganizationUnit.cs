using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Organizations;

namespace Adbp.Zero.OrganizationUnits
{
    public class ZeroOrganizationUnit: OrganizationUnit
    {
        /// <summary>
        /// 分组代码，如普通的用户组，OA的组织用户组
        /// </summary>
        public virtual string GroupCode { get; set; }
    }
}
