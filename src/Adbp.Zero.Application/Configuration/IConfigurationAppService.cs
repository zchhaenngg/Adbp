using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Adbp.Zero.Configuration.Dto;

namespace Adbp.Zero.Configuration
{
    public interface IConfigurationAppService : IApplicationService
    {
        Task ChangeUiDateAndTimeFormattingForUserAsync(ChangeUiDateAndTimeFormattingInput input);
        Task ChangeUiDateAndTimeFormattingForTenantAsync(ChangeUiDateAndTimeFormattingInput input);
        Task ChangeUiDateAndTimeFormattingForApplicationAsync(ChangeUiDateAndTimeFormattingInput input);
    }
}
