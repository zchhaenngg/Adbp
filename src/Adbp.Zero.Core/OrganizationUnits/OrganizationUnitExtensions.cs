using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Organizations;

namespace Adbp.Zero.OrganizationUnits
{
    public static class OrganizationUnitExtensions
    {
        public static bool IsStatic(this OrganizationUnit entity)
        {
            return (entity as ZeroOrganizationUnit)?.IsStatic == true;
        }

        public static bool IsStatic(this UserOrganizationUnit entity)
        {
            return (entity as ZeroUserOrganizationUnit)?.IsStatic == true;
        }
    }
}
