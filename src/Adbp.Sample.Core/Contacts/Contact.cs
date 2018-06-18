using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adbp.Domain.Entities;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Sample.Contacts
{
    public class Contact : FullAuditedTOEntity<long, User>
    {
        public const int MaxTelephoneLength = 11;
        public const int MaxEmailLength = 255;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual bool? IsFemale { get; set; }

        [StringLength(MaxTelephoneLength)]
        public virtual string Telephone { get; set; }

        [StringLength(MaxEmailLength)]
        public virtual string Email { get; set; }
    }
}
