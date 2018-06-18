using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Adbp.Paging.Dto;
using Adbp.Sample.Authorization;
using Adbp.Sample.Contacts.Dto;
using Adbp.Zero.SysObjectSettings;

namespace Adbp.Sample.Contacts
{
    [AbpAuthorize(SamplePermissionNames.Permissions_Contact)]
    public class ContactAppService : SampleAppServiceBase, IContactAppService
    {
        private readonly IRepository<Contact, long> _contactRepository;

        public ContactAppService(
            IRepository<Contact, long> contactRepository,
            SysObjectSettingManager sysObjectSettingManager)
            : base(sysObjectSettingManager)
        {
            _contactRepository = contactRepository;
        }

        public virtual async Task CreateContactAsync(CreateContactDto input)
        {
            await CreateAsync(_contactRepository, input);
        }

        public virtual async Task DeleteAsync(long contactId)
        {
            await DeleteAsync(_contactRepository, contactId);
        }

        public virtual async Task<ContactDto> GetContactDtoAsync(long contactId)
        {
            return await GetAsync<Contact, long, ContactDto>(_contactRepository, contactId);
        }

        public virtual async Task<PagedResultDto<ContactDto>> GetContactDtosAsync(GenericPagingInput input = null)
        {
            return await GetAllAsync<Contact, long, ContactDto>(_contactRepository, input);
        }

        public virtual async Task UpdateContactAsync(UpdateContactDto input)
        {
            await UpdateAsync(_contactRepository, input);
        }
    }
}
