using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Adbp.Paging.Dto;

namespace Adbp.Application.Services
{
    public interface IAdbpCrudAppService<TPrimaryKey, TEntityDto, TCreateInput, TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        Task<TEntityDto> GetAsync(TPrimaryKey id);
        Task CreateAsync(TCreateInput input);
        Task UpdateAsync(TUpdateInput input);
        Task DeleteAsync(TPrimaryKey id);
        Task<PagedResultDto<TEntityDto>> GetAllAsync(GenericPagingInput input);
    }
}
