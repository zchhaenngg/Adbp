using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Authorization.Roles.Dto
{
    public class RolePermissionDto
    {
        public int RoleId { get; set; }
        public string PermissionName { get; set; }
        public bool IsStatic { get; set; }
        public bool IsGranted { get; set; }
    }
}
