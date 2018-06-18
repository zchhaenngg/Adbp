using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Adbp.Paging.Dto;
using Adbp.Sample.Contacts.Dto;

namespace Adbp.Sample.Contacts
{
    public interface IContactAppService: IApplicationService
    {
        Task<ContactDto> GetContactDtoAsync(long contactId);
        Task CreateContactAsync(CreateContactDto input);
        Task UpdateContactAsync(UpdateContactDto input);
        Task DeleteAsync(long contactId);
        Task<PagedResultDto<ContactDto>> GetContactDtosAsync(GenericPagingInput input = null);
    }
}
