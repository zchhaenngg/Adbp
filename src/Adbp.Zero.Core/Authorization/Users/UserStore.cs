using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Adbp.Zero.Authorization.Roles;

namespace Adbp.Zero.Authorization.Users
{
    public class UserStore : UserStore<Role, User>
    {
        public UserStore(IRepository<User, long> userRepository, 
            IRepository<UserLogin, long> userLoginRepository, 
            IRepository<UserRole, long> userRoleRepository, 
            IRepository<Role> roleRepository, 
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository, 
            IUnitOfWorkManager unitOfWorkManager, 
            IRepository<UserClaim, long> userClaimRepository) 
            : base(userRepository,
                  userLoginRepository,
                  userRoleRepository, 
                  roleRepository, 
                  userPermissionSettingRepository, 
                  unitOfWorkManager, 
                  userClaimRepository)
        {
        }
    }

    public abstract class UserStore<TRole, TUser> : AbpUserStore<TRole, TUser>
        where TRole : Role<TUser>
        where TUser : User<TUser>
    {
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public UserStore(
            IRepository<TUser, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<TRole> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserClaim, long> userClaimRepository)
            : base(
                  userRepository,
                  userLoginRepository,
                  userRoleRepository,
                  roleRepository,
                  userPermissionSettingRepository,
                  unitOfWorkManager,
                  userClaimRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public virtual async Task AddToRoleAsync(TUser user, int roleId)
        {
            await _userRoleRepository.InsertAsync(new UserRole(user.TenantId, user.Id, roleId));
        }

        public virtual async Task RemoveFromRoleAsync(TUser user, int roleId)
        {
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == roleId);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }
    }
}
