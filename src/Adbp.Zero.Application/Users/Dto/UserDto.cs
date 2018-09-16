using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        public string UserName { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public int[] RoleIds { get; set; }

        public bool IsStatic { get; set; }

        public DateTime? LastModificationTime { get; set; }


        #region reserve fields
        public virtual string Field1 { get; set; }
        public virtual string Field2 { get; set; }
        public virtual string Field3 { get; set; }
        public virtual string Field4 { get; set; }
        public virtual string Field5 { get; set; }
        public virtual string Field6 { get; set; }
        public virtual string Field7 { get; set; }
        public virtual string Field8 { get; set; }
        public virtual string Field9 { get; set; }
        public virtual string Field10 { get; set; }
        public virtual string Field11 { get; set; }
        public virtual string Field12 { get; set; }
        public virtual string Field13 { get; set; }
        public virtual string Field14 { get; set; }
        public virtual string Field15 { get; set; }
        #endregion
    }
}
