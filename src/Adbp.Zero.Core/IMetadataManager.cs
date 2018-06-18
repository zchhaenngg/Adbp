using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Services;

namespace Adbp.Zero
{
    public interface IMetadataManager
    {
        Task<List<string>> GetSysObjectNamesAsync();
        Task<List<string>> GetSysColumnNamesAsync(string table);
    }
}
