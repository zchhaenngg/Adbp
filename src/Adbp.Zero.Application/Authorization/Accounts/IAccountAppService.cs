using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Adbp.Zero.Authorization.Accounts.Dto;

namespace Adbp.Zero.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);
        Task ChangePassword(ChangePasswordInput input);
    }
}
