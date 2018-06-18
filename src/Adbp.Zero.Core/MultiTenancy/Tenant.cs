using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MultiTenancy;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
    }
}
