using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Adbp.Sample.Contacts.Dto
{
    [AutoMapFrom(typeof(Contact))]
    public class ContactDto : EntityDto<long>
    {
        public string Name { get; set; }

        public bool? IsFemale { get; set; }
        
        public string Telephone { get; set; }

        public string Email { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
