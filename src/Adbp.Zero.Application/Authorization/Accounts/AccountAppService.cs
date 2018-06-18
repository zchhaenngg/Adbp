using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Adbp.Zero.Authorization.Accounts.Dto;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.MultiTenancy;
using Abp.IdentityFramework;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero.Authorization.Accounts
{
    public class AccountAppService : ZeroAppServiceBase, IAccountAppService
    {
        private readonly UserManager _userManager;

        public AccountAppService(
            UserManager userManager,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _userManager = userManager;
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            var result = await _userManager.ChangePasswordAsync(AbpSession.GetUserId(), input.CurrentPassword, input.NewPassword);
            result.CheckErrors(LocalizationManager);
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            Tenant tenant = await Task.FromResult<Tenant>(null);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }
    }
}
