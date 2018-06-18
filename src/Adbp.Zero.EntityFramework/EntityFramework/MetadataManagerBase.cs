using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Adbp.Zero;

namespace Adbp.Zero.EntityFramework
{
    public abstract class MetadataManagerBase : IMetadataManager
    {
        private readonly DbContext _dbContext;

        protected MetadataManagerBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<string>> GetSysObjectNamesAsync()
        {
            return _dbContext.Database.SqlQuery<string>("SELECT NAME FROM SYSOBJECTS WHERE TYPE='U' AND NAME LIKE 'C_%'").ToListAsync();
        }

        public Task<List<string>> GetSysColumnNamesAsync(string table)
        {
            return _dbContext.Database.SqlQuery<string>("SELECT NAME FROM SYSCOLUMNS WHERE ID=OBJECT_ID(@p0)", table).ToListAsync();
        }
    }
}
