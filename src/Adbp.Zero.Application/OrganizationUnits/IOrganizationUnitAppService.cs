using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Zero.OrganizationUnits.Dto;
using Adbp.Paging.Dto;

namespace Adbp.Zero.OrganizationUnits
{
    public interface IOrganizationUnitAppService: IApplicationService
    {
        Task<IList<OrganizationUnitOutput>> GetOrganizationUnitsAsync();
        Task<PagedResultDto<OrganizationUnitUserDto>> GetOrganizationUnitUserPageAsync(GenericPagingInput input, long organizationUnitId);
        Task<PagedResultDto<OrganizationUserOuput>> GetUsersNotInOrganizationAsync(GenericPagingInput input);


        Task<OrganizationUnitOutput> CreateOrganizationUnitAsync(CreateOrganizationUnitInput input);
        Task<OrganizationUnitOutput> UpdateOrganizationUnitAsync(UpdateOrganizationUnitInput input);
        Task DeleteOrganizationUnitAsync(long Id);

        Task AddOrganizationUnitUserAsync(long organizationUnitId, long userid);
        Task AddOrganizationUnitUsersAsync(IList<OrganizationUnitUserInput> list);
        Task DeleteOrganizationUnitUserAsync(long organizationUnitId, long userid);
        Task DeleteOrganizationUnitUsersAsync(IList<OrganizationUnitUserInput> list);
    }
}
