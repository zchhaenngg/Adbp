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
using Adbp.Zero.Configuration;
using Abp.Configuration;

namespace Adbp.Zero.OrganizationUnits
{
    [AbpAuthorize(ZeroPermissionNames.Permissions_OrganizationUnit)]
    public class OrganizationUnitAppService : ZeroAppServiceBase, IOrganizationUnitAppService
    {
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<ZeroUserOrganizationUnit, long> _zeroUserOrganizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly ZeroOrganizationUnitManager _zeroOrganizationUnitManager;
        private readonly UserManager _userManager;

        public OrganizationUnitAppService(
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<ZeroUserOrganizationUnit, long> zeroUserOrganizationUnit,
            IRepository<User, long> userRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            OrganizationUnitManager organizationUnitManager,
            ZeroOrganizationUnitManager zeroOrganizationUnitManager,
            UserManager userManager,
            SysObjectSettingManager sysObjectSettingManager
            ) : base(sysObjectSettingManager)
        {
            _organizationUnitRepository = organizationUnitRepository;
            _zeroUserOrganizationUnitRepository = zeroUserOrganizationUnit;
            _userRepository = userRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitManager = organizationUnitManager;
            _zeroOrganizationUnitManager = zeroOrganizationUnitManager;
            _userManager = userManager;
        }
        
        protected OrganizationUnitOutput Converter(OrganizationUnit organizationUnit)
        {
            var output = Map<OrganizationUnitOutput>(organizationUnit);
            var zeroOrganizationUnit = organizationUnit as ZeroOrganizationUnit;
            if (zeroOrganizationUnit != null)
            {
                output.Comments = zeroOrganizationUnit.Comments;
                output.IsStatic = zeroOrganizationUnit.IsStatic;
                output.GroupCode = zeroOrganizationUnit.GroupCode;
            }
            return output;
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OrganizationUnit_Retrieve)]
        public virtual async Task<IList<OrganizationUnitOutput>> GetOrganizationUnitsAsync()
        {
            var entities = await _organizationUnitRepository.GetAllListAsync();

            return entities.ConvertAll(Converter);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OrganizationUnit_Create)]
        public virtual async Task<OrganizationUnitOutput> CreateOrganizationUnitAsync(CreateOrganizationUnitInput input)
        {
            await _zeroOrganizationUnitManager.CheckEnableOrganizationUnitManagement();
            if (input.ParentId == null)
            {
                await _zeroOrganizationUnitManager.CheckCanAddRootOrganizationUnitAsync();
            }
            else
            {
                var parent = await _organizationUnitRepository.GetAsync(input.ParentId.Value);

                await _zeroOrganizationUnitManager.CheckCanAddChildOrganizationUnitInStaticOrganizationUnitAsync(parent);
                await _zeroOrganizationUnitManager.CheckMaxOrganizationUnitDepthAsync(parent);
            }

            var displayName = input.DisplayName.Trim();
            var entity = await _organizationUnitRepository.FirstOrDefaultAsync(o => o.DisplayName == displayName && o.ParentId == input.ParentId);
            if (entity != null)
            {
                throw new UserFriendlyException($"The suborganization { displayName } is duplicate!");
            }

            entity = new ZeroOrganizationUnit { DisplayName = displayName, ParentId = input.ParentId };
            await _organizationUnitManager.CreateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return Map<OrganizationUnitOutput>(entity);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OrganizationUnit_Update)]
        public virtual async Task<OrganizationUnitOutput> UpdateOrganizationUnitAsync(UpdateOrganizationUnitInput input)
        {
            var entity = _organizationUnitRepository.Get(input.Id);
            if (entity.IsStatic())
            {
                throw new Exception("The display name of static organizations are not modifiable!");
            }
            entity.DisplayName = input.DisplayName.Trim();
            await _organizationUnitManager.UpdateAsync(entity);
            return Map<OrganizationUnitOutput>(entity);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OrganizationUnit_Delete)]
        public virtual async Task DeleteOrganizationUnitAsync(long Id)
        {
            var entity = _organizationUnitRepository.Get(Id);
            if (entity.IsStatic())
            {
                throw new Exception("The static organizations can not be deleted!");
            }
            await _organizationUnitManager.DeleteAsync(Id);
        }
        
        [AbpAuthorize(ZeroPermissionNames.Permissions_OuUser_Retrieve)]
        public virtual async Task<PagedResultDto<OrganizationUnitUserDto>> GetOrganizationUnitUserPageAsync(GenericPagingInput input, long organizationUnitId)
        {
            return await GetOrganizationUnitUserDtosAsync(input, organizationUnitId);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OuUser_Retrieve)]
        public virtual async Task<PagedResultDto<OrganizationUserOuput>> GetUsersNotInOrganizationAsync(GenericPagingInput input, long organizationUnitId)
        {
            var uouQueryable = _userOrganizationUnitRepository.GetAll();
            var queryable = _userRepository.GetAll().Where(x => !uouQueryable.Any(uou => uou.UserId == x.Id && uou.OrganizationUnitId == organizationUnitId));
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
        
        [AbpAuthorize(ZeroPermissionNames.Permissions_OuUser_Create)]
        public virtual async Task AddOrganizationUnitUserAsync(long organizationUnitId, long userid)
        {
            var ouEntity = await _organizationUnitRepository.GetAsync(organizationUnitId);
            await _zeroOrganizationUnitManager.CheckCanAddUserInStaticOrganizationUnitAsync(ouEntity);

            var entity = await _userOrganizationUnitRepository.FirstOrDefaultAsync(x => x.OrganizationUnitId == organizationUnitId && x.UserId == userid);
            if (entity == null)
            {
                await _userOrganizationUnitRepository.InsertAsync(new ZeroUserOrganizationUnit
                {
                    UserId = userid,
                    OrganizationUnitId = organizationUnitId
                });
            }
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OuUser_Create)]
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

        [AbpAuthorize(ZeroPermissionNames.Permissions_OuUser_Delete)]
        public virtual async Task DeleteOrganizationUnitUserAsync(long organizationUnitId, long userid)
        {
            var entity = await _userOrganizationUnitRepository.SingleAsync(x => x.OrganizationUnitId == organizationUnitId && x.UserId == userid);
            if (entity.IsStatic())
            {
                throw new Exception("The static UserOrganizationUnit can not be deleted!");
            }
            await _userOrganizationUnitRepository.DeleteAsync(x => x.OrganizationUnitId == organizationUnitId && x.UserId == userid);
        }

        [AbpAuthorize(ZeroPermissionNames.Permissions_OuUser_Delete)]
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
                            join zuou in _zeroUserOrganizationUnitRepository.GetAll() on uou.Id equals zuou.Id into zuouInto
                            from zuou in zuouInto.DefaultIfEmpty() //连接zou表, 如果zou没匹配上时, 使用默认值代替
                            select new OrganizationUnitUserDto
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                Name = user.Name,
                                Surname = user.Surname,
                                CreationTime = uou.CreationTime,
                                CreatorUserId = uou.CreatorUserId,
                                OrganizationUnitId = ou.Id,
                                OrganizationUnitName = ou.DisplayName,
                                IsStatic = zuou.IsStatic
                            };
            if (organizationUnitId != null)
            {
                queryable = queryable.Where(x => x.OrganizationUnitId == organizationUnitId);
            }
            return await GetAll<OrganizationUnitUserDto, long, GenericPagingInput, OrganizationUnitUserDto>(queryable, input);

        }
    }
}
