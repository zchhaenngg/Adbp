using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Adbp.Zero.Analasy.Dto;
using Adbp.Zero.Authorization;
using Adbp.Paging.Dto;
using Adbp.Zero.Authorization.Users;
using Newtonsoft.Json;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero.Analasy
{
    [AbpAuthorize]
    public class AnalasyAppService : ZeroAppServiceBase, IAnalasyAppService
    {
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;
        private readonly IRepository<User, long> _userRepository;

        public AnalasyAppService(
            IRepository<AuditLog, long> auditLogRepository,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IRepository<User, long> userRepository,
            SysObjectSettingManager sysObjectSettingManager
            ) :base(sysObjectSettingManager)
        {
            _auditLogRepository = auditLogRepository;
            _userLoginAttemptRepository = userLoginAttemptRepository;
            _userRepository = userRepository;
        }

        [AbpAuthorize(PermissionNames.Permissions_AuditLog)]
        public virtual async Task<PagedResultDto<AuditLogDto>> GetAuditLogsAsync(GenericPagingInput input)
        {
            var page = await GetAll<AuditLog, long, GenericPagingInput, AuditLogDto>(_auditLogRepository.GetAll(), input);
            var users = await _userRepository.GetAllListAsync();
            foreach (AuditLogDto item in page.Items)
            {
                if (item.UserId != null)
                {
                    item.UserStr = users.FirstOrDefault(x => x.Id == item.UserId)?.UserStr;
                }
            }
            return page;
        }

        [AbpAuthorize(PermissionNames.Permissions_LoginAttemptLog)]
        public virtual async Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttemptsAsync(GenericPagingInput input)
        {
            var page = await GetAll<UserLoginAttempt, long, GenericPagingInput, UserLoginAttemptDto>(_userLoginAttemptRepository.GetAll(), input);
            return page;
        }
        
        public virtual async Task<PagedResultDto<UserLoginAttemptDto>> GetSelfLoginAttemptsAsync(GenericPagingInput input)
        {
            long userId = AbpSession.GetUserId();
            var page = await GetAll<UserLoginAttempt, long, GenericPagingInput, UserLoginAttemptDto>(
                _userLoginAttemptRepository.GetAll().Where(x=>x.UserId == userId), 
                input);
            return page;
        }
    }
}
