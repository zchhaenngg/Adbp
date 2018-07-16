using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Authorization.Roles
{
    public class Role : Role<User>
    {
        //please writing on below
    }

    public abstract class Role<TUser> : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        public const int MaxDescriptionLength = 5000;

        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
