using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.MultiTenancy
{
    public class Tenant : Tenant<User>
    {
        //please writing on below
    }

    public abstract class Tenant<TUser> : AbpTenant<TUser>
        where TUser : AbpUserBase
    {

    }
}
