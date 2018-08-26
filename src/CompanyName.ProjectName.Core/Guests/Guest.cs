using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Adbp.Domain.Entities;
using Adbp.Zero.Authorization.Users;

namespace CompanyName.ProjectName.Guests
{
    public class Guest : FullAuditedTOEntity<long, User>
    {
        public const int MaxTelephoneLength = 11;
        /// <summary>
        /// 嘉宾姓名
        /// </summary>
        [StringLength(50)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 中文拼音
        /// </summary>
        [StringLength(100)]
        public virtual string PinYin { get; set; }

        /// <summary>
        /// 是否为女性
        /// </summary>
        public virtual bool? IsFemale { get; set; }

        /// <summary>
        /// 院长、主任、专家
        /// </summary>
        [StringLength(100)]
        public virtual string Title { get; set; }

        /// <summary>
        /// 单位/公司
        /// </summary>
        [StringLength(200)]
        public virtual string Company { get; set; }

        [StringLength(500)]
        public virtual string Description { get; set; }

        [StringLength(MaxTelephoneLength)]
        public virtual string Telephone { get; set; }

        [StringLength(40)]
        public virtual string Guid { get; set; }
    }
}
