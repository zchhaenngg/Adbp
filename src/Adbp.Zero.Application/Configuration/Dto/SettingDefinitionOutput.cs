using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adbp.Zero.Configuration.Dto
{
    public class SettingGroupOutput
    {
        public string GroupName { get; set; }
        public string GroupDisplay { get; set; }
    }
    public class SettingDefinitionOutput: SettingGroupOutput
    {
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
}
