using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Adbp.Zero.Authorization.Roles;
using Adbp.Zero.Authorization.Users;

namespace Adbp.Zero.Authorization.Roles
{
    public class Role : AbpRole<User>
    {
        public const int MaxDescriptionLength = 5000;

        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
