using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Authorization.Roles.Dto
{
    public class SetPermissionsInput
    {
        /// <summary>
        /// role id
        /// </summary>
        public int Id { get; set; }
        
        public List<string> Permissions { get; set; }
    }
}
