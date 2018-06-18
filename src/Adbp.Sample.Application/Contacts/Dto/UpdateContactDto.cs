using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Adbp.Sample.Contacts.Dto
{
    [AutoMapTo(typeof(Contact))]
    public class UpdateContactDto : EntityDto<long>
    {
        [Required]
        public string Name { get; set; }

        public bool? IsFemale { get; set; }

        [StringLength(11)]
        public string Telephone { get; set; }
        
        public string Email { get; set; }
    }
}
