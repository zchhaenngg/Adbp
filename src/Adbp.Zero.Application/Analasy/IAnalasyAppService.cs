using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Zero.Analasy.Dto;
using Adbp.Paging.Dto;

namespace Adbp.Zero.Analasy
{
    public interface IAnalasyAppService: IApplicationService
    {
        Task<PagedResultDto<AuditLogDto>> GetAuditLogsAsync(GenericPagingInput input);
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttemptsAsync(GenericPagingInput input);
        Task<PagedResultDto<UserLoginAttemptDto>> GetSelfLoginAttemptsAsync(GenericPagingInput input);
    }
}
