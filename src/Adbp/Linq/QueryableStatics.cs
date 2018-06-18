using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Adbp.Paging.Dto;
using Adbp.Linq.Expressions;
using Abp.Linq;

namespace Adbp.Linq
{
    public static class QueryableStatics
    {
        public static NullAsyncQueryableExecuter AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        /// <summary>
        /// Should apply sorting if needed(implement interface ISortedResultRequest).
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        public static IQueryable<TEntity> ApplySorting<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> query, TGetAllInput input = null)
            where TEntity: class, IEntity<TPrimaryKey>
            where TGetAllInput: class
        {
            //Try to sort query if available
            var sortInput = input as ISortedResultRequest;
            if (sortInput != null)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            //IQueryable.Task requires sorting, so we should sort if Take will be used.
            if (input is ILimitedResultRequest)
            {
                return query.OrderByDescending(e => e.Id);
            }

            //No sorting
            return query;
        }

        /// <summary>
        /// Should apply paging if needed(implement interface IPagedResultRequest，ILimitedResultRequest).
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="input">The input.</param>
        public static IQueryable<TEntity> ApplyPaging<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> query, TGetAllInput input = null)
            where TEntity : IEntity<TPrimaryKey>
            where TGetAllInput: class
        {
            //Try to use paging if available
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }

        /// <summary>
        /// Should apply where if needed(input is not null.)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <typeparam name="TGetAllInput"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyWhere<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> queryable, TGetAllInput input = null)
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class, IPageQueryItems
        {
            if (input ==null || input.QueryItems == null || input.QueryItems.Count == 0)
            {
                return queryable;
            }
            var list = input.QueryItems.Select(x => x as IExpressionGroupItem).ToList();
            var predicate = PredicateBuilder.Create<TEntity>(list);
            return queryable.Where(predicate);
        }

        /// <summary>
        /// TGetAllInput，如果实现了排序接口（ISortedResultRequest）则排序，如果实现了分页接口（IPagedResultRequest，ILimitedResultRequest）则分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <typeparam name="TGetAllInput"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetAll<TEntity, TPrimaryKey, TGetAllInput>(IQueryable<TEntity> queryable,
            TGetAllInput input = null)
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class, IPageQueryItems
        {
            var query = ApplyWhere<TEntity, TPrimaryKey, TGetAllInput>(queryable, input);
            query = ApplySorting<TEntity, TPrimaryKey, TGetAllInput>(query, input);
            query = ApplyPaging<TEntity, TPrimaryKey, TGetAllInput>(query, input);
            return await AsyncQueryableExecuter.ToListAsync(query);
        }

        public static async Task<PagedResultDto<TEntityDto>> GetAll<TEntity, TPrimaryKey, TGetAllInput, TEntityDto>(
            IQueryable<TEntity> queryable,
            Func<TEntity, TEntityDto> selector,
            TGetAllInput input = null
            )
            where TEntity : class, IEntity<TPrimaryKey>
            where TGetAllInput : class, IPageQueryItems
        {
            var query = ApplyWhere<TEntity, TPrimaryKey, TGetAllInput>(queryable, input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting<TEntity, TPrimaryKey, TGetAllInput>(query, input);
            query = ApplyPaging<TEntity, TPrimaryKey, TGetAllInput>(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(selector).ToList()
            );
        }
    }
}
