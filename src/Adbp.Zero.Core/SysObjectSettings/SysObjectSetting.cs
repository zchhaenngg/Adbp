using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using Adbp.Domain.Entities;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.SysObjectSettings
{
    /// <summary>
    /// Owner和Creator可以无条件修改其所有数据！（万一，不允许Owner和Creator修改自己的数据，可以通过配置权限甚至编码的方式处理）
    /// OrganizationUnitId和RoleId 互斥
    /// 字段优先级 Field > Table 即访问表所有字段和其中某个字段的配置都存在时，该字段以Field为准
    /// </summary>
    public class SysObjectSetting : AuditedEntity<long, User>, IMustHaveTenant
    {
        public virtual long? OrganizationUnitId { get; set; }

        public virtual OrganizationUnit OrganizationUnit { get; set; }

        public virtual int? RoleId { get; set; }

        public virtual Role Role { get; set; }

        [Required]
        [StringLength(128)]
        public virtual string SysObjectName { get; set; }

        [StringLength(128)]
        public virtual string SysColumnName { get; set; }

        /// <summary>
        /// AccessLevel = AccessLevel.Retrieve | AccessLevel.Update
        ///  表示 Field字段（如果Field为空则表示Table的所有字段）只有Retrieve和Update的访问权利，没有Create和Delete的访问权利
        /// </summary>
        public virtual AccessLevel AccessLevel { get; set; }
        public int TenantId { get; set; }
    }
    
    public enum AccessLevel
    {
        /// <summary>
        /// 无任何权限
        /// </summary>
        Reject = 1,//不能为0， 因为HasFlag会始终为true
        Create = 2,
        Retrieve = 4,
        Update = 8,
        Delete = 16
    }
}
