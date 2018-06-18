using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.OrganizationUnits.Dto
{
    public class CreateOrganizationUnitInput
    {
        [Required]
        public string DisplayName { get; set; }
        public long? ParentId { get; set; }
    }
}
