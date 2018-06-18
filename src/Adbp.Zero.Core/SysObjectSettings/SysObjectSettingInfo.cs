using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.SysObjectSettings
{
    /// <summary>
    /// do not care Setting come from（Ou/Role）
    /// </summary>
    public class SysObjectSettingInfo
    {
        public string SysObjectName { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
