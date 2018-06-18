using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Adbp.Zero.Authorization.Roles;
using Microsoft.AspNet.Identity;

namespace Adbp.Zero.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly UserStore _userStore;

        public UserManager(
            IRepository<User, long> userRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,

            UserStore userStore,
            RoleManager roleManager, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings, 
            ILocalizationManager localizationManager, 
            IdentityEmailMessageService emailService, 
            ISettingManager settingManager, 
            IUserTokenProviderAccessor userTokenProviderAccessor) 
            : base(
                  userStore, 
                  roleManager, 
                  permissionManager, 
                  unitOfWorkManager, 
                  cacheManager, 
                  organizationUnitRepository, 
                  userOrganizationUnitRepository, 
                  organizationUnitSettings, 
                  localizationManager, 
                  emailService, 
                  settingManager, 
                  userTokenProviderAccessor)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userStore = userStore;
        }

        public async Task<List<User>> GetUsersInRoleAsync(string roleName)
        {
            var queryable = from user in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                         join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                         where role.Name == roleName
                         select user;
            return await NullAsyncQueryableExecuter.Instance.ToListAsync(queryable.Distinct());
        }

        public async Task<List<User>> GetUsersInRoleAsync(int roleId)
        {
            var queryable = from user in _userRepository.GetAll()
                         join userRole in _userRoleRepository.GetAll() on user.Id equals userRole.UserId
                         where userRole.RoleId == roleId
                         select user;
            return await NullAsyncQueryableExecuter.Instance.ToListAsync(queryable.Distinct());
        }

        public async Task<List<User>> GetUsersNotInRoleIdAsync(int roleId)
        {
            var urQueryable = _userRoleRepository.GetAll().Where(ur => ur.RoleId == roleId);
            var queryable = _userRepository.GetAll().Where(x => 
                urQueryable.All(ur => ur.UserId != x.Id)
                );
            return await NullAsyncQueryableExecuter.Instance.ToListAsync(queryable.Distinct());
        }

        public virtual async Task<IdentityResult> SetRoles(User user, int[] roleIds)
        {
            if (roleIds == null)
            {
                return await SetRoles(user, new string[0]);
            }
            else
            {
                var roleNames = _roleRepository.GetAll().Where(x => roleIds.Contains(x.Id)).Select(x => x.Name).ToArray();
                return await SetRoles(user, roleNames);
            }
        }

        public virtual async Task AddToRoleAsync(long userId, int roleId)
        {
            var user = await _userRepository.GetAsync(userId);
            await AddToRoleAsync(user, roleId);
        }

        public virtual async Task AddToRoleAsync(User user, int roleId)
        {
            await _userStore.AddToRoleAsync(user, roleId);
        }

        public virtual async Task RemoveFromRoleAsync(User user, int roleId)
        {
            await _userStore.RemoveFromRoleAsync(user, roleId);
        }

        public virtual async Task RemoveFromRoleAsync(long userId, int roleId)
        {
            var user = await _userRepository.GetAsync(userId);
            await RemoveFromRoleAsync(user, roleId);
        }
    }
}
