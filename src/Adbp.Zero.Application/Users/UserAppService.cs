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
using Abp.Notifications;
using Adbp.Zero.Notifications;
using Abp;
using Abp.Runtime.Session;

namespace Adbp.Zero.Users
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_User)]
    public class UserAppService : ZeroAppServiceBase, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly INotificationPublisher _notiticationPublisher;
        private readonly IRepository<LoginAgent, long> _loginAgentRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public UserAppService(
            IRepository<LoginAgent, long> loginAgentRepository,
            IRepository<User, long> userRepository,
            IRepository<Role> roleRepository,
            UserManager userManager,
            RoleManager roleManager,

            INotificationPublisher notiticationPublisher,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _loginAgentRepository = loginAgentRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _notiticationPublisher = notiticationPublisher;
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
            UnitOfWorkManager.Current.SaveChanges();
            await _notiticationPublisher.PublishAsync(ZeroNotificationNames.NewUserRegistered, data: CreateNotificationData(user.Id), severity: NotificationSeverity.Success);

            //var userIdentifier = new Abp.Domain.Entities.EntityIdentifier(typeof(User), user.Id);
            //await _notiticationPublisher.PublishAsync(ZeroNotificationNames.NewUserRegistered, severity: NotificationSeverity.Success, 
            //    entityIdentifier: userIdentifier); //entityIdentifier 实际上没有发生订阅
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

        private static NotificationData CreateNotificationData(long id)
        {
            var notificationData = new NotificationData();
            notificationData["Id"] = id;
            return notificationData;
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_UserAgent_Upsert)]
        public async Task AddAgentAsync(long principalId, long agentId)
        {
            await _loginAgentRepository.InsertAsync(new LoginAgent
            {
                AgentId = agentId,
                PrincipalId = principalId
            });
        }

        public async Task AddAgentAsync(long agentId)
        {
            await AddAgentAsync(AbpSession.GetUserId());
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_UserAgent_Upsert)]
        public async Task RemoveAgentAsync(long principalId, long agentId)
        {
            await _loginAgentRepository.DeleteAsync(x => x.PrincipalId == principalId && x.AgentId == agentId);
        }

        public async Task RemoveAgentAsync(long agentId)
        {
            await RemoveAgentAsync(AbpSession.GetUserId(), agentId);
        }

        [AbpAllowAnonymous]
        public async Task<List<UserDto>> GetAgentsAsync()
        {
            var userId = AbpSession.GetUserId();
            var users = await AsyncQueryableExecuter.ToListAsync(_loginAgentRepository.GetAll().Where(x => x.PrincipalId == userId).Select(x => x.Agent));
            return users.Select(Map<UserDto>).ToList();
        }
    }
}
