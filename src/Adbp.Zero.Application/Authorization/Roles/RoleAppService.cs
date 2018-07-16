using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Adbp.Zero.Authorization.Roles.Dto;
using Microsoft.AspNet.Identity;
using Abp.IdentityFramework;
using Abp.UI;
using Adbp.Zero.Authorization.Users;
using Abp.Authorization.Users;
using Adbp.Zero.Users.Dto;
using Abp.Authorization;
using Adbp.Paging.Dto;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero.Authorization.Roles
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_Role)]
    public class RoleAppService : ZeroAppServiceBase, IRoleAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;

        public RoleAppService(
            IRepository<User, long> userRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            UserManager userManager,
            RoleManager roleManager,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_Role_Retrieve)]
        public virtual async Task<PagedResultDto<RoleDto>> GetRoles(GenericPagingInput input = null)
        {
            return await GetAll<Role, int, GenericPagingInput, RoleDto>(
                _roleRepository.GetAll(), input);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_Role_Create)]
        public virtual async Task CreateRoleAsync(CreateRoleDto input)
        {
            var role = ObjectMapper.Map<Role>(input);
            CheckErrors(await _roleManager.CreateAsync(role));

            UnitOfWorkManager.Current.SaveChanges();

            var grantedPermissions = PermissionManager
               .GetAllPermissions()
               .Where(p => input.Permissions?.Contains(p.Name) == true)
               .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
        
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public virtual Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();
            return Task.FromResult(new ListResultDto<PermissionDto>(
               ObjectMapper.Map<List<PermissionDto>>(permissions)
           ));
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_Role_Retrieve)]
        public virtual async Task<UpdateRoleDto> GetUpdateRoleDto(int roleId)
        {
            var role = await _roleManager.GetRoleByIdAsync(roleId);
            return Map<UpdateRoleDto>(role);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_Role_Update)]
        public virtual async Task UpdateRoleAsync(UpdateRoleDto input)
        {
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var name = role.Name;
            Map(input, role);
            role.Name = name;

            CheckErrors(await _roleManager.UpdateAsync(role));
            if (!role.IsStatic)
            {
                var grantedPermissions = PermissionManager
                .GetAllPermissions()
                .Where(p => input.Permissions?.Contains(p.Name) == true)
                .ToList();

                await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
            }
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_Role_Delete)]
        public virtual async Task Delete(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role.IsStatic)
            {
                throw new UserFriendlyException("Cannot Delete A Static Role");
            }
            
            var users = await _userManager.GetUsersInRoleAsync(roleId);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user.Id, role.Name));
            }
            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_Role_Retrieve)]
        public virtual async Task<RoleDto> GetRoleDto(int roleId)
        {
            var entity = await _roleRepository.GetAsync(roleId);
            return Map<RoleDto>(entity);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_UserRole_Retrieve)]
        public virtual async Task<List<UserDto>> GetUserDtosInRole(int roleId)
        {
            var list =  await _userManager.GetUsersInRoleAsync(roleId);
            return Map<List<UserDto>>(list); 
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_UserRole_Retrieve)]
        public virtual async Task<List<UserDto>> GetUserDtosNotInRole(int roleId)
        {
            var list = await _userManager.GetUsersNotInRoleIdAsync(roleId);
            return Map<List<UserDto>>(list);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_UserRole_Upsert)]
        public virtual async Task AddToRoleAsync(long userId, int roleId)
        {
            await _userManager.AddToRoleAsync(userId, roleId);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_UserRole_Upsert)]
        public virtual async Task RemoveFromRoleAsync(long userId, int roleId)
        {
            await _userManager.RemoveFromRoleAsync(userId, roleId);
        }
    }
}
