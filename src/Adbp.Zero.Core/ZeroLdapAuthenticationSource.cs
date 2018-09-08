using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
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

        protected override void UpdateUserFromPrincipal(User user, UserPrincipal userPrincipal)
        {
            if (!userPrincipal.SamAccountName.IsNullOrEmpty())
            {
                user.UserName = userPrincipal.SamAccountName;
            }
            user.EmailAddress = userPrincipal.EmailAddress;
            if (userPrincipal.Enabled.HasValue)
            {
                user.IsActive = userPrincipal.Enabled.Value;
            }
            if (user.Name.IsNullOrWhiteSpace())
            {
                user.Name = userPrincipal.GivenName;
                user.Surname = userPrincipal.Surname;
            }
        }
    }
}
