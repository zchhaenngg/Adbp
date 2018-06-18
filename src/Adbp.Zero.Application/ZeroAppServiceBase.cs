using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Adbp.Linq.Expressions;
using Abp.Linq.Extensions;
using Adbp.Paging.Dto;
using Adbp.Linq;
using Adbp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Runtime.Session;
using Adbp.Zero.SysObjectSettings;
using Abp.Authorization;
using Abp.UI;

namespace Adbp.Zero
{
    public abstract class ZeroAppServiceBase : ApplicationService
    {
        private readonly SysObjectSettingManager _sysObjectSettingManager;

        public NullAsyncQueryableExecuter AsyncQueryableExecuter { get; private set; }
       

        protected ZeroAppServiceBase(SysObjectSettingManager sysObjectSettingManager)
        {
            LocalizationSourceName = ZeroConsts.LocalizationSourceName;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            _sysObjectSettingManager = sysObjectSettingManager;
        }

        protected virtual string GetSysObjectName<TEntity>()
            where TEntity : class
        {
            return $"{AdbpConsts.TableConsts.CUSTOM}{typeof(TEntity).Name}";
        }

        protected virtual async Task<List<TEntity>> GetAll<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> queryable,
            TGetAllInput input = null)
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class, IPageQueryItems
        {
            return await QueryableStatics.GetAll<TEntity, TPrimaryKey, TGetAllInput>(queryable, input);
        }

        protected virtual async Task<PagedResultDto<TEntityDto>> GetAll<TEntity, TPrimaryKey, TGetAllInput, TEntityDto>(
            IQueryable<TEntity> queryable,
            TGetAllInput input = null
            )
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class, IPageQueryItems
        {
            return await QueryableStatics.GetAll<TEntity, TPrimaryKey, TGetAllInput, TEntityDto>(queryable, Map<TEntityDto>, input);
        }

        protected virtual IQueryable<TEntity> ApplyWhere<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> queryable, TGetAllInput input = null)
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class, IPageQueryItems
        {
            return QueryableStatics.ApplyWhere<TEntity, TPrimaryKey, TGetAllInput>(queryable, input);
        }

        /// <summary>
        /// Should apply sorting if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplySorting<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> query, TGetAllInput input = null)
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class
        {
            return QueryableStatics.ApplySorting<TEntity, TPrimaryKey, TGetAllInput>(query, input);
        }

        /// <summary>
        /// Should apply paging if needed.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        protected virtual IQueryable<TEntity> ApplyPaging<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> query, TGetAllInput input = null)
            where TEntity : IEntity<TPrimaryKey>
            where TGetAllInput : class
        {
            return QueryableStatics.ApplyPaging<TEntity, TPrimaryKey, TGetAllInput>(query, input);
        }

        /// <summary>
        /// Execute a mapping from the source object to the existing destination object
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <returns>Returns the same destination object after mapping operation</returns>
        protected virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return ObjectMapper.Map(source, destination);
        }

        /// <summary>
        /// Converts an object to another. Creates a new object of TDestination.
        /// </summary>
        /// <typeparam name="TDestination">Type of the destination object</typeparam>
        /// <param name="source">Source object</param>
        /// <returns></returns>
        protected virtual TDestination Map<TDestination>(object source)
        {
            return ObjectMapper.Map<TDestination>(source);
        }

        /// <summary>
        /// Throws Abp.AbpException if Abp.Runtime.Session.IAbpSession.UserId
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual bool IsOwner<TEntity>(TEntity entity)
            where TEntity : IMayHaveOwner
        {
            if (entity == null)
            {
                return false;
            }
            return entity.OwnerId == AbpSession.GetUserId();
        }

        /// <summary>
        /// Throws Abp.AbpException if Abp.Runtime.Session.IAbpSession.UserId
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual bool IsCreator<TEntity>(TEntity entity)
            where TEntity : ICreationAudited
        {
            if (entity == null)
            {
                return false;
            }
            return entity.CreatorUserId == AbpSession.GetUserId();
        }

        protected virtual async Task<Expression<Func<TEntity, bool>>> GetLoginFilter<TEntity>(string sysObjectName)
            where TEntity : class
        {
            //能查看所有记录的配置信息,    Table Type User 
            if (await _sysObjectSettingManager.HasRetrieveAccessLevelAsync(AbpSession.GetUserId(), sysObjectName))
            {
                return o => true;
            }
            Expression<Func<TEntity, bool>> predicate = o => false;
            if (typeof(ICreationAudited).IsAssignableFrom(typeof(TEntity)))
            {
                predicate = ExpressionBuilder.Create(predicate, nameof(ICreationAudited.CreatorUserId), AbpSession.GetUserId().ToString(), ExpressionOperate.Equal, false);
            }
            if (typeof(IMayHaveOwner).IsAssignableFrom(typeof(TEntity)))
            {
                predicate = ExpressionBuilder.Create(predicate, nameof(IMayHaveOwner.OwnerId), AbpSession.GetUserId().ToString(), ExpressionOperate.Equal, false);
            }
            return predicate;
        }

        protected virtual void CheckCreatePermission<TEntity>(TEntity entity)
            where TEntity : class
        {
            PermissionChecker.Authorize($"Permissions.{typeof(TEntity).Name}.Create");
        }

        protected virtual void CheckRetrievePermission<TEntity>(TEntity entity)
            where TEntity : class
        {
            PermissionChecker.Authorize($"Permissions.{typeof(TEntity).Name}.Retrieve");

            if (IsOwner(entity as IMayHaveOwner))
            {
                return;
            }
            if (IsCreator(entity as ICreationAudited))
            {
                return;
            }
            throw new UserFriendlyException("You do not have access to this data.");
        }

        protected virtual void CheckUpdatePermission<TEntity>(TEntity entity)
            where TEntity : class
        {
            PermissionChecker.Authorize($"Permissions.{typeof(TEntity).Name}.Update");

            if (IsOwner(entity as IMayHaveOwner))
            {
                return;
            }
            if (IsCreator(entity as ICreationAudited))
            {
                return;
            }
            throw new UserFriendlyException("You do not have permission to manipulate this data.");
        }

        protected virtual void CheckDeletePermission<TEntity>(TEntity entity)
        {
            PermissionChecker.Authorize($"Permissions.{typeof(TEntity).Name}.Delete");

            if (IsOwner(entity as IMayHaveOwner))
            {
                return;
            }
            if (IsCreator(entity as ICreationAudited))
            {
                return;
            }
            throw new UserFriendlyException("You do not have permission to manipulate this data.");
        }

        protected virtual void CheckPagePermission<TEntity>()
        {
            PermissionChecker.Authorize($"Permissions.{typeof(TEntity).Name}");
        }

        //AdbpCrudAppServiceBase GetAsync
        protected virtual async Task<TEntityDto> GetAsync<TEntity, TPrimaryKey, TEntityDto>(IRepository<TEntity, TPrimaryKey> repository, TPrimaryKey id)
            where TEntityDto : IEntityDto<TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>

        {
            var entity = await repository.GetAsync(id);
            CheckRetrievePermission(entity);
            return Map<TEntityDto>(entity);
        }

        protected virtual async Task CreateAsync<TEntity, TPrimaryKey, TCreateInput>(IRepository<TEntity, TPrimaryKey> repository, TCreateInput input)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var entity = Map<TEntity>(input);
            CheckCreatePermission(entity);
            await repository.InsertAsync(entity);
        }

        protected virtual async Task UpdateAsync<TEntity, TPrimaryKey, TUpdateInput>(IRepository<TEntity, TPrimaryKey> repository, TUpdateInput input)
            where TEntity : class, IEntity<TPrimaryKey>
            where TUpdateInput : IEntityDto<TPrimaryKey>
        {
            var entity = await repository.GetAsync(input.Id);
            CheckUpdatePermission(entity);
            Map(input, entity);
            await repository.UpdateAsync(entity);
        }

        protected virtual async Task DeleteAsync<TEntity, TPrimaryKey>(IRepository<TEntity, TPrimaryKey> repository, TPrimaryKey id)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var entity = await repository.GetAsync(id);
            CheckDeletePermission(entity);
            await repository.DeleteAsync(entity);
        }

        protected virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync<TEntity, TPrimaryKey, TEntityDto>(IRepository<TEntity, TPrimaryKey> repository, GenericPagingInput input = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            CheckPagePermission<TEntity>();
            var predicate = await GetLoginFilter<TEntity>(GetSysObjectName<TEntity>());
            return await GetAll<TEntity, TPrimaryKey, GenericPagingInput, TEntityDto>(
                repository.GetAll().Where(predicate), input);
        }
    }
}
