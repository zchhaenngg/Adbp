using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Adbp.Paging.Dto;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Zero
{
    public abstract class ZeroCrudAppServiceBase<TEntity, TPrimaryKey, TEntityDto, TCreateInput, TUpdateInput> : ZeroAppServiceBase, IAdbpCrudAppService<TPrimaryKey, TEntityDto, TCreateInput, TUpdateInput>
        where TEntity: class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected readonly IRepository<TEntity, TPrimaryKey> _Repository;

        protected ZeroCrudAppServiceBase(
            IRepository<TEntity, TPrimaryKey> repository,
            SysObjectSettingManager sysObjectSettingManager) 
            : base(sysObjectSettingManager)
        {
            _Repository = repository;
        }

        public virtual async Task<TEntityDto> GetAsync(TPrimaryKey id)
        {
            return await GetAsync<TEntity, TPrimaryKey, TEntityDto>(_Repository, id);
        }

        public virtual async Task CreateAsync(TCreateInput input)
        {
            await CreateAsync(_Repository, input);
        }

        public virtual async Task UpdateAsync(TUpdateInput input)
        {
            await UpdateAsync(_Repository, input);
        }

        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            await DeleteAsync(_Repository, id);
        }

        public virtual async Task<PagedResultDto<TEntityDto>> GetAllAsync(GenericPagingInput input = null)
        {
            return await GetAllAsync<TEntity, TPrimaryKey, TEntityDto>(_Repository, input);
        }
    }
}
