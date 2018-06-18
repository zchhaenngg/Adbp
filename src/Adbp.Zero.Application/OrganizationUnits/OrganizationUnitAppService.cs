using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.UI;
using Adbp.Zero.Authorization.Users;
using Adbp.Zero.OrganizationUnits.Dto;
using Adbp.Paging.Dto;
using Adbp.Zero.Authorization;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero.OrganizationUnits
{
    [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit)]
    public class OrganizationUnitAppService : ZeroAppServiceBase, IOrganizationUnitAppService
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly UserManager _userManager;

        public OrganizationUnitAppService(
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<User, long> userRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            OrganizationUnitManager organizationUnitManager,
            UserManager userManager,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _organizationUnitRepository = organizationUnitRepository;
            _userRepository = userRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _userManager = userManager;
        }

        [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit_Retrieve)]
        public virtual async Task<IList<OrganizationUnitOutput>> GetOrganizationUnitsAsync()
        {
            var entities = await _organizationUnitRepository.GetAllListAsync();
            return Map<List<OrganizationUnitOutput>>(entities);
        }

        [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit_Retrieve)]
        public virtual async Task<PagedResultDto<OrganizationUnitUserDto>> GetOrganizationUnitUserPageAsync(GenericPagingInput input, long organizationUnitId)
        {
            return await GetOrganizationUnitUserDtosAsync(input, organizationUnitId);
        }

        [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit_Retrieve)]
        public virtual async Task<PagedResultDto<OrganizationUserOuput>> GetUsersNotInOrganizationAsync(GenericPagingInput input)
        {
            var uouQueryable = _userOrganizationUnitRepository.GetAll();
            var queryable = _userRepository.GetAll().Where(x => !uouQueryable.Any(uou => uou.UserId == x.Id));
            queryable = ApplyWhere<User, long, GenericPagingInput>(queryable, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(queryable);
            queryable = ApplySorting<User, long, GenericPagingInput>(queryable, input);
            queryable = ApplyPaging<User, long, GenericPagingInput>(queryable, input);
            var items = (await AsyncQueryableExecuter.ToListAsync(queryable)).Select(x => new OrganizationUserOuput
            {
                UserId = x.Id,
                UserName = x.UserName,
                Name = x.Name,
                Surname = x.Surname,
                LastModificationTime = x.LastModificationTime ?? x.CreationTime
            }).ToList();
            return new PagedResultDto<OrganizationUserOuput>(totalCount, items);
        }

        [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit_Create)]
        public virtual async Task<OrganizationUnitOutput> CreateOrganizationUnitAsync(CreateOrganizationUnitInput input)
        {
            var displayName = input.DisplayName.Trim();
            if (_organizationUnitRepository.GetAll().Any(o => o.DisplayName == displayName && o.ParentId == input.ParentId))
            {
                throw new UserFriendlyException("不能重复创建组织！");
            }

            var entity = new OrganizationUnit { DisplayName = displayName, ParentId = input.ParentId };
            await _organizationUnitManager.CreateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return Map<OrganizationUnitOutput>(entity);
        }

        [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit_Update)]
        public virtual async Task<OrganizationUnitOutput> UpdateOrganizationUnitAsync(UpdateOrganizationUnitInput input)
        {
            var entity = _organizationUnitRepository.Get(input.Id);
            entity.DisplayName = input.DisplayName.Trim();
            await _organizationUnitManager.UpdateAsync(entity);
            return Map<OrganizationUnitOutput>(entity);
        }

        [AbpAuthorize(PermissionNames.Permissions_OrganizationUnit_Delete)]
        public virtual async Task DeleteOrganizationUnitAsync(long Id)
        {
            await _organizationUnitManager.DeleteAsync(Id);
        }
        public virtual async Task AddOrganizationUnitUserAsync(long organizationUnitId, long userid)
        {
            var entity = await _userOrganizationUnitRepository.FirstOrDefaultAsync(x => x.OrganizationUnitId == organizationUnitId && x.UserId == userid);
            if (entity == null)
            {
                await _userOrganizationUnitRepository.InsertAsync(new UserOrganizationUnit
                {
                    UserId = userid,
                    OrganizationUnitId = organizationUnitId
                });
            }
        }
        public virtual async Task AddOrganizationUnitUsersAsync(IList<OrganizationUnitUserInput> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    await AddOrganizationUnitUserAsync(item.OrganizationUnitId, item.UserId);
                }
            }
        }
        public virtual async Task DeleteOrganizationUnitUserAsync(long organizationUnitId, long userid)
        {
            await _userOrganizationUnitRepository.DeleteAsync(x => x.OrganizationUnitId == organizationUnitId && x.UserId == userid);
        }
        public virtual async Task DeleteOrganizationUnitUsersAsync(IList<OrganizationUnitUserInput> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    await DeleteOrganizationUnitUserAsync(item.OrganizationUnitId, item.UserId);
                }
            }
        }
        
        /// <summary>
        /// 内部调用，不加权限控制
        /// </summary>
        /// <param name="input"></param>
        /// <param name="organizationUnitId"></param>
        /// <returns></returns>
        protected virtual async Task<PagedResultDto<OrganizationUnitUserDto>> GetOrganizationUnitUserDtosAsync(GenericPagingInput input, long? organizationUnitId)
        {
            var queryable = from user in _userRepository.GetAll()
                            join uou in _userOrganizationUnitRepository.GetAll() on user.Id equals uou.UserId
                            join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                            select new OrganizationUnitUserDto
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                Name = user.Name,
                                Surname = user.Surname,
                                CreationTime = uou.CreationTime,
                                CreatorUserId = uou.CreatorUserId,
                                OrganizationUnitId = ou.Id,
                                OrganizationUnitName = ou.DisplayName
                            };
            if (organizationUnitId != null)
            {
                queryable = queryable.Where(x => x.OrganizationUnitId == organizationUnitId);
            }
            return await GetAll<OrganizationUnitUserDto, long, GenericPagingInput, OrganizationUnitUserDto>(queryable, input);
        }
    }
}
