using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Organizations;
using Adbp.Domain.Entities;

namespace Adbp.Zero.OrganizationUnits
{
    public class ZeroOrganizationUnit : OrganizationUnit, IReserveFields
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

        [StringLength(255)]
        public virtual string Field1 { get; set; }

        [StringLength(255)]
        public virtual string Field2 { get; set; }

        [StringLength(255)]
        public virtual string Field3 { get; set; }

        [StringLength(255)]
        public virtual string Field4 { get; set; }

        [StringLength(255)]
        public virtual string Field5 { get; set; }

        [StringLength(255)]
        public virtual string Field6 { get; set; }

        [StringLength(255)]
        public virtual string Field7 { get; set; }

        [StringLength(255)]
        public virtual string Field8 { get; set; }

        [StringLength(255)]
        public virtual string Field9 { get; set; }

        [StringLength(255)]
        public virtual string Field10 { get; set; }

        [StringLength(255)]
        public virtual string Field11 { get; set; }

        [StringLength(255)]
        public virtual string Field12 { get; set; }

        [StringLength(255)]
        public virtual string Field13 { get; set; }

        [StringLength(255)]
        public virtual string Field14 { get; set; }

        [StringLength(255)]
        public virtual string Field15 { get; set; }
    }
}
