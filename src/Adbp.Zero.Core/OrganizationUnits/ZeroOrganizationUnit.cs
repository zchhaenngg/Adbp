using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Organizations;

namespace Adbp.Zero.OrganizationUnits
{
    public class ZeroOrganizationUnit : OrganizationUnit
    {
        /// <summary>
        /// 分组代码，如普通的用户组，OA的组织用户组
        /// </summary>
        [StringLength(50)]
        public virtual string GroupCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public virtual string Comments { get; set; }

        /// <summary>
        /// true，则不可修改
        /// </summary>
        public virtual bool IsStatic { get; set; }
    }
}
