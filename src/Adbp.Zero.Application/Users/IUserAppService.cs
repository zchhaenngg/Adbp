using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Paging.Dto;
using Adbp.Zero.Users.Dto;

namespace Adbp.Zero.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserDto>> GetUsers(GenericPagingInput input);
        Task CreateUserAsync(CreateUserDto input);
        Task UpdateUserAsync(UpdateUserDto input);
        Task Delete(int userId);

        Task AddAgentAsync(long principalId, long agentId);
        Task AddAgentAsync(long agentId);
        Task RemoveAgentAsync(long principalId, long agentId);
        Task RemoveAgentAsync(long agentId);

        Task<List<UserDto>> GetAgentsAsync();
        
    }
}