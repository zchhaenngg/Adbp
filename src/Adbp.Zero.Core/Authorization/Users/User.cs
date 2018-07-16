using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Extensions;
using Microsoft.AspNet.Identity;

namespace Adbp.Zero.Authorization.Users
{
    public class User : User<User>
    {
        //please writing on below
    }

    public class User<TUser> : AbpUser<TUser>
        where TUser: AbpUser<TUser>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public string UserStr => $"{FullName}({UserName})";
    }
}
