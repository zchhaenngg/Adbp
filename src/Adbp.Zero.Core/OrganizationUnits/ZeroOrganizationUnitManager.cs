using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;
using Abp.Organizations;
using Adbp.Zero.Configuration;
using Abp.Configuration;

namespace Adbp.Zero.OrganizationUnits
{
    public class ZeroOrganizationUnitManager : DomainService
    {
        public ZeroOrganizationUnitManager()
        {

        }

        public virtual async Task<bool> IsEnableOrganizationUnitManagement()
        {
            return await SettingManager.GetSettingValueAsync<bool>(ZeroSettingNames.OrganizationSettings.EnableOrganizationUnitManagement);
        }

        public virtual async Task CheckEnableOrganizationUnitManagement()
        {
            if (!await IsEnableOrganizationUnitManagement())
            {
                throw new Exception("Enable OrganizationUnit Management -> false");
            }
        }

        public virtual async Task CheckCanAddRootOrganizationUnitAsync()
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(ZeroSettingNames.OrganizationSettings.CanAddRootOrganizationUnit))
            {
                throw new Exception("Can Add Root OrganizationUnit -> false");
            }
        }

        public virtual async Task CheckCanAddChildOrganizationUnitInStaticOrganizationUnitAsync(OrganizationUnit parent)
        {
            if (parent.IsStatic())
            {
                if (!await SettingManager.GetSettingValueAsync<bool>(ZeroSettingNames.OrganizationSettings.CanAddChildOrganizationUnitInStaticOrganizationUnit))
                {
                    throw new Exception("Can Add Child OrganizationUnit In Static OrganizationUnit -> false");
                }
            }
        }

        public virtual async Task CheckMaxOrganizationUnitDepthAsync(OrganizationUnit parent)
        {
            int maxDepth = await SettingManager.GetSettingValueAsync<int>(ZeroSettingNames.OrganizationSettings.MaxOrganizationUnitDepth);
            if (parent.Code.Split('.').Length >= maxDepth)
            {
                throw new Exception($"The Max OrganizationUnit Depth -> {maxDepth}");
            }
        }

        public virtual async Task CheckCanAddUserInStaticOrganizationUnitAsync(OrganizationUnit entity)
        {
            if (entity.IsStatic())
            {
                if (!await SettingManager.GetSettingValueAsync<bool>(ZeroSettingNames.OrganizationSettings.CanAddUserInStaticOrganizationUnit))
                {
                    throw new Exception("Can Add User In Static OrganizationUnit -> false");
                }
            }
        }
    }
}
