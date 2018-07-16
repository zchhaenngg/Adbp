using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Adbp.Zero.Authorization.Users;
using Adbp.Paging.Dto;
using Adbp.Zero.Users.Dto;
using Abp.IdentityFramework;
using System.Collections.ObjectModel;
using Abp.Authorization.Users;
using Adbp.Zero.Authorization.Roles;
using Abp.Authorization;
using Adbp.Zero.Authorization;
using Adbp.Zero.SysObjectSettings;
using Abp.UI;

namespace Adbp.Zero.Users
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_User)]
    public class UserAppService : ZeroAppServiceBase, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public UserAppService(
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            UserManager userManager,
            RoleManager roleManager,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_User)]
        public virtual async Task<PagedResultDto<UserDto>> GetUsers(GenericPagingInput input)
        {
            return await GetAll<User, long, GenericPagingInput, UserDto>(_userRepository.GetAll(), input);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_User_Create)]
        public virtual async Task CreateUserAsync(CreateUserDto input)
        {
            var user = Map<User>(input);
            user.TenantId = AbpSession.TenantId;
            user.Password = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword(input.Password.Trim());
            user.IsEmailConfirmed = true;

            if (await PermissionChecker.IsGrantedAsync(ZeroPermissionNames.Permissions_UserRole_Upsert))
            {
                if (input.RoleIds != null)
                {
                    user.Roles = new Collection<UserRole>();
                    foreach (var roleId in input.RoleIds)
                    {
                        var role = await _roleManager.GetRoleByIdAsync(roleId);
                        user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
                    }
                }
            }
            (await _userManager.CreateAsync(user)).CheckErrors(LocalizationManager);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_User_Delete)]
        public virtual async Task Delete(int userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_User_Update)]
        public virtual async Task UpdateUserAsync(UpdateUserDto input)
        {
            var user = Map(input, await _userManager.GetUserByIdAsync(input.Id));
            (await _userManager.UpdateAsync(user)).CheckErrors(LocalizationManager);

            if (await PermissionChecker.IsGrantedAsync(ZeroPermissionNames.Permissions_UserRole_Upsert))
            {
                (await _userManager.SetRoles(user, input.RoleIds)).CheckErrors(LocalizationManager);
            }
        }
    }
}
