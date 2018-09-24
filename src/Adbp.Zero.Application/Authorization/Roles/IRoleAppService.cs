using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Zero.Authorization.Roles.Dto;
using Adbp.Paging.Dto;
using Adbp.Zero.Users.Dto;

namespace Adbp.Zero.Authorization.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task<RoleDto> GetRoleDto(int roleId);
        Task<List<UserDto>> GetUserDtosInRole(int roleId);
        Task<List<UserDto>> GetUserDtosNotInRole(int roleId);
        Task<ListResultDto<PermissionDto>> GetAllPermissions();
        Task<PagedResultDto<RoleDto>> GetRoles(GenericPagingInput input = null);
        Task CreateRoleAsync(CreateRoleDto input);
        Task<UpdateRoleDto> GetUpdateRoleDto(int roleId);
        Task UpdateRoleAsync(UpdateRoleDto input);
        Task Delete(int roleId);
        Task AddToRoleAsync(long userId, int roleId);
        Task RemoveFromRoleAsync(long userId, int roleId);
        Task SetPermissionsAsync(SetPermissionsInput input);
        Task<List<RolePermissionDto>> GetRolePermissionDtosAsync(int roleId);
    }
}
