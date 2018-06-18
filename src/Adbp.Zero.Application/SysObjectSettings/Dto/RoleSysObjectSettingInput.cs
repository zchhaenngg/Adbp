using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Adbp.Zero.SysObjectSettings.Dto
{
    [AutoMapTo(typeof(SysObjectSetting))]
    public class RoleSysObjectSettingInput
    {
        public int RoleId { get; set; }

        [Required]
        public string SysObjectName { get; set; }
        
        /// <summary>
        /// AccessLevel = AccessLevel.Retrieve | AccessLevel.Update
        ///  表示 Field字段（如果Field为空则表示Table的所有字段）只有Retrieve和Update的访问权利，没有Create和Delete的访问权利
        /// </summary>
        public AccessLevel AccessLevel { get; set; }
    }
}
