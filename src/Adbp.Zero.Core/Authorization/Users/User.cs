using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Extensions;
using Adbp.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace Adbp.Zero.Authorization.Users
{
    public class User : User<User>
    {
        //please writing on below

    }

    public class User<TUser> : AbpUser<TUser>, IReserveFields
        where TUser: AbpUser<TUser>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public virtual bool IsStatic { get; set; } 

        public virtual string UserStr => $"{FullName}({UserName})";

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
