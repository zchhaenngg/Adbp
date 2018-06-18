using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.SysObjectSettings.Dto
{
    public class SysObjectSettingInput
    {
        [Required]
        public string SysObjectName { get; set; }

        public string SysColumnName { get; set; }

        /// <summary>
        /// AccessLevel = AccessLevel.Retrieve | AccessLevel.Update
        ///  表示 Field字段（如果Field为空则表示Table的所有字段）只有Retrieve和Update的访问权利，没有Create和Delete的访问权利
        /// </summary>
        public AccessLevel AccessLevel { get; set; }
        public int AccessLevelInt => (int)AccessLevel;//客户端需要转换成int,进行或运算，以便知道各个字段的选中情况
    }
}
