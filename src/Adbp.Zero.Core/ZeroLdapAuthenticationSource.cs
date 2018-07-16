using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.MultiTenancy;

namespace Adbp.Zero
{
    public class ZeroLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public ZeroLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
             : base(settings, ldapModuleConfig)
        {

        }
    }
}
